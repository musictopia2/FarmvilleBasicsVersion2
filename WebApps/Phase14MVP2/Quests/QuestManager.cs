namespace Phase14MVP2.Quests;
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

        _quests.RemoveAllAndObtain(x =>
            x.LevelRequired < _currentLevel ||
            x.Status == EnumQuestStatus.Completed);

        // NEW: if runtime errors ever caused over-generation, fix the board now
        NormalizeBoardPerLevelCaps();

        // then fill back up to 20 (this will naturally spill into next levels)
        FillBoardTo20();

        await SaveQuestsAsync();
    }
    private async Task SaveQuestsAsync()
    {
        await _questProfile.SaveAsync(_quests);
    }
    private void NormalizeBoardPerLevelCaps()
    {
        // Enforce: for each level, number of ACTIVE quests on the board
        // must be <= allowedForLevel (current level uses points remaining, future uses PreviewLevelPoints).
        //
        // If overflow exists, remove some quests for that level so FillBoardTo20() can
        // generate replacements for later levels.

        // Safety guards so we never infinite-loop due to bad preview logic.
        const int maxLevelScan = 500;

        int level = _currentLevel;

        for (int i = 0; i < maxLevelScan; i++, level++)
        {
            // figure out cap for this level
            int allowedForLevel =
                (level == _currentLevel)
                    ? (progressionManager.PointsRequired - progressionManager.CurrentPoints)
                    : progressionManager.PreviewLevelPoints(level);

            if (allowedForLevel < 0)
            {
                allowedForLevel = 0;
            }

            // snapshot quests for this level (ACTIVE only)
            var atLevel = _quests
                .Where(q => q.Status == EnumQuestStatus.Active && q.LevelRequired == level)
                .ToList();

            int overflow = atLevel.Count - allowedForLevel;
            if (overflow <= 0)
            {
                // If there are no quests at this level and we’re already beyond the highest level
                // present on the board, we can stop scanning.
                // (This avoids scanning 500 levels when your board only has current+1.)
                bool anyAbove = _quests.Any(q => q.Status == EnumQuestStatus.Active && q.LevelRequired > level);
                if (!anyAbove)
                {
                    break;
                }
                continue;
            }

            // Choose which ones to remove:
            // Prefer removing non-tracked first, and among those:
            // - remove NOT completable first (so we keep ready-to-turn-in ones)
            // - then remove lowest progress
            // Then, if still overflow, remove tracked (oldest tracked first).
            var removalOrder = atLevel
                .OrderBy(q => q.Tracked ? 1 : 0)                  // non-tracked first
                .ThenBy(q => CanCompleteQuest(q) ? 1 : 0)         // not completable first
                .ThenBy(q => Progress01(q))                       // lowest progress first
                .ThenBy(q => q.Tracked ? q.Order : int.MaxValue)  // if tracked, oldest first
                .ToList();

            for (int r = 0; r < overflow && r < removalOrder.Count; r++)
            {
                _quests.RemoveSpecificItem(removalOrder[r]);
            }

            // Continue loop — there might be overflow at later levels too.
        }
    }
    private void FillBoardTo20()
    {
        if (_quests.Count >= 20)
        {
            return;
        }

        bool initialFill = _quests.Count == 0;

        // count ACTIVE quests per level (don’t count completed)
        var perLevelCounts = _quests
            .Where(q => q.Status == EnumQuestStatus.Active)
            .GroupBy(q => q.LevelRequired)
            .ToDictionary(g => g.Key, g => g.Count());

        int level = _currentLevel;

        // how many we still need to ADD (never more than 20)
        int addsRemaining = 20 - _quests.Count;

        // safety: we also cap total loop iterations so we can’t hang the UI
        int steps = 0;
        const int maxSteps = 200;

        while (addsRemaining > 0 && steps++ < maxSteps)
        {
            // NEW: If we’re at/above the max level, stop early.
            // This allows the board to remain < 20 at the end of the game.
            if (progressionManager.IsEnd(level))
            {
                break;
            }

            int allowedForLevel = GetAllowedForLevelOrZero(level);

            perLevelCounts.TryGetValue(level, out int existingForLevel);

            if (existingForLevel >= allowedForLevel)
            {
                level++;
                continue;
            }

            var eligible = itemManager.GetEligibleItems(level);

            var boardSnapshot = _quests.ToBasicList();
            var quest = _questGenerationService.CreateQuest(level, eligible, boardSnapshot);

            quest.QuestId = Guid.NewGuid().ToString();
            quest.Seen = initialFill;
            quest.Tracked = false;
            quest.Status = EnumQuestStatus.Active;
            quest.LevelRequired = level;

            _quests.Add(quest);

            perLevelCounts[level] = existingForLevel + 1;
            addsRemaining--;
        }

        // If we hit maxSteps, fail fast instead of hanging forever
        if (steps >= maxSteps && addsRemaining > 0)
        {
            throw new CustomBasicException("FillBoardTo20 exceeded safety limit; check PreviewLevelPoints / allowed quest logic.");
        }
    }

    private int GetAllowedForLevelOrZero(int level)
    {
        if (level == _currentLevel)
        {
            // points left in current level
            return Math.Max(0, progressionManager.PointsRequired - progressionManager.CurrentPoints);
        }

        // future levels come from preview; if preview returns 0, we interpret that as "no more levels"
        // (which matches what you said about max level)
        return Math.Max(0, progressionManager.PreviewLevelPoints(level));
    }
    public BasicList<QuestInstanceModel> ShowCurrentQuests(int max = 3)
    {
        var active = _quests
            .Where(q => q.Status == EnumQuestStatus.Active)
            .ToBasicList();

        // 1) tracked first (most recent tracked first) - KEEP DUPLICATES
        var tracked = active
            .Where(q => q.Tracked)
            .OrderByDescending(q => q.Order)
            .ToBasicList();

        // If they tracked enough quests to satisfy the UI,
        // show exactly what they chose (even duplicates).
        if (tracked.Count >= max)
        {
            return tracked.Take(max).ToBasicList();
        }

        // EARLY GAME: if under level 10 and nothing is tracked,
        // don't do "smart ordering" (players aren't focusing yet).
        if (_currentLevel < 10 && tracked.Count == 0)
        {
            // show the first few active quests in board order,
            // but don't show duplicates of the same item in this shortlist
            return TakeDistinctByItemName(active, max).ToBasicList();
        }

        // 2) Fill remaining slots with suggestions.
        // Suggestions should avoid duplicates among themselves AND vs tracked items,
        // but tracked duplicates remain intact.
        var result = new BasicList<QuestInstanceModel>();
        result.AddRange(tracked);

        var usedItems = new HashSet<string>(
            tracked.Select(x => x.ItemName),
            StringComparer.OrdinalIgnoreCase
        );

        int remaining = max - result.Count;
        if (remaining <= 0)
        {
            return result.Take(max).ToBasicList();
        }

        // Candidates: not tracked, not already represented in tracked items
        var baseCandidates = active
            .Where(q => !q.Tracked)
            .Where(q => !usedItems.Contains(q.ItemName));

        // Prefer current-or-lower level quests first; only pull higher-level if needed.
        var preferredLevel = baseCandidates
            .Where(q => q.LevelRequired <= _currentLevel)
            .OrderByDescending(q => CanCompleteQuest(q)) // ready-to-turn-in first
            .ThenByDescending(q => Progress01(q));       // most progress next

        var futureLevel = baseCandidates
            .Where(q => q.LevelRequired > _currentLevel)
            .OrderByDescending(q => CanCompleteQuest(q))
            .ThenByDescending(q => Progress01(q));

        void AddFrom(IEnumerable<QuestInstanceModel> seq)
        {
            foreach (var q in seq)
            {
                if (result.Count >= max)
                {
                    break;
                }

                // avoid duplicates in the suggested portion
                if (usedItems.Add(q.ItemName))
                {
                    result.Add(q);
                }
            }
        }

        AddFrom(preferredLevel);
        AddFrom(futureLevel);

        return result.Take(max).ToBasicList();
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