namespace Phase11ProgressionVisibility.Quests;
public class QuestManager(InventoryManager inventory)
{
    private IQuestPersistence _questPersistence = null!;
    private BasicList<QuestRecipe> _quests = [];
    private int _trackedSeq = 0;
    public async Task SetStyleContextAsync(QuestServicesContext context)
    {
        if (_questPersistence != null)
        {
            throw new InvalidOperationException("Persistance Already set");
        }
        _questPersistence = context.QuestPersistence;
        _quests = await context.QuestRecipes.GetQuestsAsync();
        _trackedSeq = _quests.Count == 0 ? 0 : _quests.Max(x => x.Order);
    }


    public BasicList<QuestRecipe> ShowCurrentQuests(int max = 3)
    {
        var incomplete = _quests.Where(q => q.Completed == false).ToBasicList();
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

    private double Progress01(QuestRecipe q)
    {
        int have = inventory.Get(q.Item); // your inventory accessor
        if (q.Required <= 0)
        {
            return 0;
        }

        return Math.Min(1.0, (double)have / q.Required);
    }
    public async Task SetTrackedAsync(QuestRecipe q, bool tracked, int maxTracked = 3)
    {
        if (q.Completed)
        {
            q.Tracked = false;
            q.Order = 0;
            await _questPersistence.SaveQuestsAsync(_quests);
            return;
        }
        if (tracked == false)
        {
            q.Tracked = false;
            q.Order = 0;
            await _questPersistence.SaveQuestsAsync(_quests); //must save this.
            return;
        }

        // If already tracked, just "refresh" its recency
        if (q.Tracked)
        {
            q.Order = ++_trackedSeq;
            await _questPersistence.SaveQuestsAsync(_quests); //must save this.
            return;
        }

        // If at cap, auto-untrack the oldest tracked quest
        var trackedList = _quests
            .Where(x => x.Completed == false && x.Tracked)
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
        await _questPersistence.SaveQuestsAsync(_quests);
    }

    public BasicList<QuestRecipe> GetAllIncompleteQuests()
        => _quests.Where(x => x.Completed == false).ToBasicList();


    public bool CanCompleteQuest(QuestRecipe recipe) => inventory.Has(recipe.Item, recipe.Required);
    public async Task CompleteQuestAsync(QuestRecipe recipe)
    {
        if (CanCompleteQuest(recipe) == false)
        {
            throw new CustomBasicException("Unable to complete quest.   Should had called CanCompleteQuest first");
        }
        inventory.Consume(recipe.Item, recipe.Required);
        recipe.Completed = true;
        recipe.Tracked = false;
        recipe.Order = 0;
        inventory.Add(recipe.Rewards);
        await _questPersistence.SaveQuestsAsync(_quests);
    }
}