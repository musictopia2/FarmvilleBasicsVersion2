using Phase09LevelProgression.Services.Inventory;

namespace Phase09LevelProgression.Services.Workshops;
public class WorkshopRecipeSummary
{
    public string Item { get; init; } = "";
    public Dictionary<string, int> Inputs { get; init; } = [];
    public ItemAmount Output { get; init; }
    public TimeSpan Duration { get; init; }
}