using Phase04EnforcingLimits.Services.Inventory;

namespace Phase04EnforcingLimits.DataAccess.Workshops;
public class WorkshopRecipeDocument
{
    public string BuildingName { get; init; } = "";
    public string Item { get; init; } = "";
    public Dictionary<string, int> Inputs { get; init; } = [];
    public ItemAmount Output { get; init; }
    public TimeSpan Duration { get; init; }
    required public string Theme { get; init; }
}