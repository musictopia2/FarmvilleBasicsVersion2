namespace Phase04PrepareForMVP1.QuestHelpers;
public sealed class BasicItem
{
    public string Item { get; init; } = "";
    public TimeSpan BaseTime { get; init; }
    public int Amount { get; set; }
    public EnumItemSource Source { get; set; } //this only works for trees, animals, or crops.
}