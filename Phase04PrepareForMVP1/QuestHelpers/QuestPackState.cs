namespace Phase04PrepareForMVP1.QuestHelpers;

internal sealed class QuestPackState
{
    // ---- counts used so far ----
    public int WorksiteCount { get; private set; }
    public int RareWorksiteCount { get; private set; }

    public int MultiStepWorkshopCount { get; private set; }      // WorkshopInputs > 0
    public int RareInputWorkshopCount { get; private set; }      // WorstWorksiteInputRarity == Rare

    // ---- duplicate tracking (internal only) ----
    private readonly Dictionary<string, int> _basicCounts = new();        // key: BasicItem.Item
    private readonly Dictionary<string, int> _workshopKeyCounts = new();  // key: OutputItem or WorkshopName

    // ---- query helpers ----
    public int TimesUsedBasic(string item) =>
        _basicCounts.TryGetValue(item, out var n) ? n : 0;

    public int TimesUsedWorkshopKey(string key) =>
        _workshopKeyCounts.TryGetValue(key, out var n) ? n : 0;

    // ---- eligibility checks ----
    public bool CanTakeBasic(BasicItem b, QuestGenerationSettings s)
    {
        if (s.Limits.MaxSameBasicItemQuests <= 0) return true;
        return TimesUsedBasic(b.Item) < s.Limits.MaxSameBasicItemQuests;
    }

    public bool CanTakeWorkshop(WorkshopOutputAnalysis w, QuestGenerationSettings s)
    {
        // hard caps for complexity / rarity inputs
        if (w.WorkshopInputs > 0 && MultiStepWorkshopCount >= s.Limits.MaxMultiStepWorkshopQuests)
            return false;

        if (w.WorstWorksiteInputRarity == EnumWorksiteRarity.Rare &&
            RareInputWorkshopCount >= s.Limits.MaxRareInputWorkshopQuests)
            return false;

        // optional repeat cap for workshops (by OutputItem or WorkshopName)
        if (s.Limits.MaxSameWorkshopItemQuests > 0)
        {
            string key = s.WorkshopRepeatKeyIsOutputItem ? w.OutputItem : w.WorkshopName;
            if (TimesUsedWorkshopKey(key) >= s.Limits.MaxSameWorkshopItemQuests)
                return false;
        }

        return true;
    }

    public bool CanTakeWorksite(WorksiteOutputAnalysis ws, QuestGenerationSettings s)
    {
        if (WorksiteCount >= s.Limits.MaxWorksiteQuests)
            return false;

        if (ws.Rarity == EnumWorksiteRarity.Rare &&
            RareWorksiteCount >= s.Limits.MaxRareWorksiteQuests)
            return false;

        return true;
    }

    // ---- apply (increment counters) ----
    public void ApplyBasic(BasicItem b)
    {
        _basicCounts.TryGetValue(b.Item, out var n);
        _basicCounts[b.Item] = n + 1;
    }

    public void ApplyWorkshop(WorkshopOutputAnalysis w, QuestGenerationSettings s)
    {
        if (w.WorkshopInputs > 0) MultiStepWorkshopCount++;
        if (w.WorstWorksiteInputRarity == EnumWorksiteRarity.Rare) RareInputWorkshopCount++;

        string key = s.WorkshopRepeatKeyIsOutputItem ? w.OutputItem : w.WorkshopName;
        _workshopKeyCounts.TryGetValue(key, out var n);
        _workshopKeyCounts[key] = n + 1;
    }

    public void ApplyWorksite(WorksiteOutputAnalysis ws)
    {
        WorksiteCount++;
        if (ws.Rarity == EnumWorksiteRarity.Rare) RareWorksiteCount++;
    }
}