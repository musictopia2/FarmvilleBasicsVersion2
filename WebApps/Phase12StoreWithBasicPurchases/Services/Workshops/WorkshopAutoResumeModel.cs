namespace Phase12StoreWithBasicPurchases.Services.Workshops;
public class WorkshopAutoResumeModel
{
    public Guid Id { get; set; } = Guid.NewGuid();        // <- persistent GUID
    public int SelectedRecipeIndex { get; set; } = 0;
    public string Name { get; set; } = "";
    public BasicList<UnlockModel> SupportedItems { get; set; } = [];
    public bool Unlocked { get; set; } //needs this too now.
    public int Capacity { get; set; } = 2;
    public BasicList<CraftingAutoResumeModel> Queue { get; set; } = [];
}