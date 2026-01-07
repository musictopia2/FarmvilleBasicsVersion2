namespace Phase04PrepareForMVP1.QuestHelpers;
internal class WorksiteOutputAnalysis
{
    public string Location { get; init; } = "";
    public string Item { get; init; } = "";
    public int OutputAmount { get; init; }
    public TimeSpan Duration { get; init; }
    public int FromWorkshops { get; set; }
    public EnumWorksiteRarity Rarity { get; init; }
}