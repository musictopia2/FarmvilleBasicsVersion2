using Phase12StoreWithBasicPurchases.Services.Inventory;

namespace Phase12StoreWithBasicPurchases.Services.Workshops;
public class WorkshopRecipeSummary
{
    public string Item { get; init; } = "";
    public Dictionary<string, int> Inputs { get; init; } = [];
    public ItemAmount Output { get; init; }
    public TimeSpan Duration { get; init; }
    public bool Unlocked { get; set; }
}