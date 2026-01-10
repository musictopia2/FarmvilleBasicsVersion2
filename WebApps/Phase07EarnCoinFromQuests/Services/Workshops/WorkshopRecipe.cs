using Phase07EarnCoinFromQuests.Services.Inventory;

namespace Phase07EarnCoinFromQuests.Services.Workshops;
public class WorkshopRecipe
{
    public string BuildingName { get; init; } = "";
    public string Item { get; init; } = "";
    public Dictionary<string, int> Inputs { get; init; } = [];
    public ItemAmount Output { get; init; }
    public TimeSpan Duration { get; init; }
}