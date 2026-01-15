namespace Phase13QuestsBasedOnLevel.Quests;
public class QuestManager(InventoryManager inventoryManager,
    ItemManager itemManager,
    ProgressionManager progressionManager
    )
{
    private int _currentLevel;
    private BasicList<QuestInstanceModel> _quests = [];
    private IQuestProfile _questProfile = null!;
    private IQuestGenerationService _questGenerationService = null!;
    private int _trackedSeq = 0;
    public async Task SetStyleContextAsync(QuestServicesContext context)
    {
        _currentLevel = progressionManager.CurrentLevel;
        _quests = await context.QuestProfile.LoadAsync();
        _questProfile = context.QuestProfile;
        _questGenerationService = context.QuestGenerationService;
        await FillQuestsAsync();
        _trackedSeq = _quests.Count == 0 ? 0 : _quests.Max(x => x.Order);
    }
    private async Task FillQuestsAsync()
    {
        if (progressionManager.CompletedGame)
        {
            _quests.Clear();
            await SaveQuestsAsync();
            return;
        }
        _quests.RemoveAllAndObtain(x => x.LevelRequired < _currentLevel || x.Status == EnumQuestStatus.Completed); //since i am required to complete the quest
        FillBoardTo20();
        await SaveQuestsAsync(); //just in case.
    }
    private async Task SaveQuestsAsync()
    {
        await _questProfile.SaveAsync(_quests);
    }
    private void FillBoardTo20()
    {
        if (_quests.Count >= 20)
        {
            return;
        }

        // If the board was empty when we started filling,
        // we want the initial 20 to be "seen" to avoid 20 aqua highlights.
        bool initialFill = _quests.Count == 0;

        int level = _currentLevel;

        while (_quests.Count < 20)
        {
            // How many quests are allowed for THIS level?
            // - For current level, it's whatever points are left to level up.
            // - For future levels, it's the preview points for that level.
            int allowedForLevel =
                (level == _currentLevel)
                    ? (progressionManager.PointsRequired - progressionManager.CurrentPoints)
                    : progressionManager.PreviewLevelPoints(level);

            // How many quests do we already have on the board for this level?
            int existingForLevel = _quests.Count(x => x.LevelRequired == level);

            // Remaining slots we can still create for this level.
            int remainingForLevel = allowedForLevel - existingForLevel;

            // If no room (or negative because we already have too many),
            // go to next level and continue filling.
            if (remainingForLevel <= 0)
            {
                level++;
                continue;
            }

            // Create exactly one quest for this level, then loop.
            var eligible = itemManager.GetEligibleItems(level);
            var quest = _questGenerationService.CreateQuest(level, eligible, _quests);

            quest.QuestId = Guid.NewGuid().ToString();
            quest.Seen = initialFill;     // initial 20 = seen; later replacements = unseen (aqua)
            quest.Tracked = false;
            quest.Status = EnumQuestStatus.Active;

            // Safety: ensure it really is tagged to this level (depends on your generator)
            quest.LevelRequired = level;

            _quests.Add(quest);
        }
    }
    public BasicList<QuestInstanceModel> ShowCurrentQuests(int max = 3)
    {
        var incomplete = _quests
            .Where(q => q.Status == EnumQuestStatus.Active)
            .ToBasicList();

        // 1) tracked first (most recent tracked first)
        var tracked = incomplete
            .Where(q => q.Tracked)
            .OrderByDescending(q => q.Order) // most recent first
            .ToBasicList();

        // EARLY GAME: if under level 10 and nothing is tracked,
        // don't do "smart ordering" (players aren't focusing yet).
        if (_currentLevel < 10 && tracked.Count == 0)
        {
            // show the first few active quests in board order,
            // but don't show duplicates of the same item in this shortlist
            return TakeDistinctByItemName(incomplete, max).ToBasicList();
        }

        // 2) fill remaining slots with “best to show next” (smart ordering),
        // but NEVER suggest duplicates in the same shortlist.
        int remaining = Math.Max(0, max - tracked.Count);
        if (remaining == 0)
        {
            // If they tracked more than max, keep the top max (and still avoid duplicates just in case)
            return TakeDistinctByItemName(tracked, max).ToBasicList();
        }

        var usedItems = new HashSet<string>(
            tracked.Select(x => x.ItemName),
            StringComparer.OrdinalIgnoreCase
        );

        var candidates = incomplete
            .Where(q => !q.Tracked)
            .Where(q => !usedItems.Contains(q.ItemName)) // prevent duplicates vs tracked
            .OrderByDescending(q => CanCompleteQuest(q)) // ready-to-turn-in first
            .ThenByDescending(q => Progress01(q));        // most progress next

        foreach (var q in candidates)
        {
            if (tracked.Count >= max)
            {
                break;
            }

            // prevent duplicates within the fill itself
            if (usedItems.Add(q.ItemName))
            {
                tracked.Add(q);
            }
        }

        // final safety: make sure we never return duplicates
        return TakeDistinctByItemName(tracked, max).ToBasicList();
    }

    private static IEnumerable<QuestInstanceModel> TakeDistinctByItemName(
        IEnumerable<QuestInstanceModel> source,
        int take
    )
    {
        var used = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var q in source)
        {
            if (used.Add(q.ItemName))
            {
                yield return q;
                take--;
                if (take <= 0)
                {
                    yield break;
                }
            }
        }
    }
    private double Progress01(QuestInstanceModel q)
    {
        int have = inventoryManager.Get(q.ItemName); // your inventory accessor
        if (q.Required <= 0)
        {
            return 0;
        }

        return Math.Min(1.0, (double)have / q.Required);
    }

    public async Task MarkAllIncompleteSeenAsync()
    {
        bool changed = false;

        foreach (var q in _quests.Where(x => x.Status == EnumQuestStatus.Active))
        {
            if (q.Seen == false)
            {
                q.Seen = true;
                changed = true;
            }
        }

        if (changed)
        {
            await SaveQuestsAsync();
        }
    }

    public BasicList<QuestInstanceModel> GetAllIncompleteQuests()
        => _quests.Where(x => x.Status == EnumQuestStatus.Active).ToBasicList();
    public bool CanCompleteQuest(QuestInstanceModel recipe) => inventoryManager.Has(recipe.ItemName, recipe.Required);
    public async Task CompleteQuestAsync(QuestInstanceModel quest)
    {
        if (CanCompleteQuest(quest) == false)
        {
            throw new CustomBasicException("Unable to complete quest.   Should had called CanCompleteQuest first");
        }
        inventoryManager.Consume(quest.ItemName, quest.Required);
        quest.Status = EnumQuestStatus.Completed;
        quest.Tracked = false;
        quest.Order = 0;
        inventoryManager.Add(quest.Rewards);
        await progressionManager.AddPointSinglePointAsync();
        _currentLevel = progressionManager.CurrentLevel; //just in case you leveled up.
        await FillQuestsAsync();
    }

    public async Task SetTrackedAsync(QuestInstanceModel q, bool tracked, int maxTracked = 3)
    {
        if (q.Status == EnumQuestStatus.Completed)
        {
            q.Tracked = false;
            q.Order = 0;
            await SaveQuestsAsync();
            return;
        }
        if (tracked == false)
        {
            q.Tracked = false;
            q.Order = 0;
            await SaveQuestsAsync();
            return;
        }

        // If already tracked, just "refresh" its recency
        if (q.Tracked)
        {
            q.Order = ++_trackedSeq;
            await SaveQuestsAsync();
            return;
        }

        // If at cap, auto-untrack the oldest tracked quest
        var trackedList = _quests
            .Where(x => x.Status == EnumQuestStatus.Active && x.Tracked)
            .OrderBy(x => x.Order) // oldest first
            .ToList();

        if (trackedList.Count >= maxTracked)
        {
            var toUntrack = trackedList[0];
            toUntrack.Tracked = false;
            toUntrack.Order = 0;
        }

        // Track the new one as most recent
        q.Tracked = true;
        q.Order = ++_trackedSeq;
        await SaveQuestsAsync();
    }
}