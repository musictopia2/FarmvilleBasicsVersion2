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
            return; //i think okay at this point.
        }
        int currentLevel;
        currentLevel = _currentLevel;
        int pointsLeft = progressionManager.PointsRequired - progressionManager.CurrentPoints;
        var count = _quests.Count(x => x.LevelRequired == currentLevel);

        pointsLeft -= count;
        if (pointsLeft == 0)
        {
            throw new CustomBasicException("Can't be next level");
        }
        do
        {
            if (_quests.Count >= 20)
            {
                return; //i think okay at this point.
            }
            var list = itemManager.GetEligibleItems(currentLevel);
            var quest = _questGenerationService.CreateQuest(currentLevel, list, _quests);
            quest.QuestId = Guid.NewGuid().ToString(); //i think should be here.  this should generate this id.
            quest.Seen = false;
            quest.Tracked = false;
            quest.Status = EnumQuestStatus.Active; //i think (?)

            _quests.Add(quest);
            pointsLeft--;
            if (pointsLeft == 0)
            {
                currentLevel++;
                pointsLeft = progressionManager.PreviewLevelPoints(currentLevel);
            }
        } while (true);
    }
    public BasicList<QuestInstanceModel> ShowCurrentQuests(int max = 3)
    {
        var incomplete = _quests.Where(q => q.Status == EnumQuestStatus.Active).ToBasicList();
        var tracked = incomplete
            .Where(q => q.Tracked)
            .OrderBy(q => q.Order) // most recent tracked first
            .ToBasicList();

        // 2) fill remaining slots with “best to show next”
        int remaining = Math.Max(0, max - tracked.Count);

        if (remaining > 0)
        {
            var fill = incomplete
                .Where(q => !q.Tracked)
                .OrderByDescending(CanCompleteQuest) // ready-to-turn-in first
                .ThenByDescending(Progress01)        // most progress next
                .Take(remaining);

            tracked.AddRange(fill);
        }

        return tracked.Take(max).ToBasicList();
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
        inventoryManager.Consume(quest.Rewards);
        await progressionManager.AddPointSinglePointAsync();
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