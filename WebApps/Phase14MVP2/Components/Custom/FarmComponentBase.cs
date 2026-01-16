namespace Phase14MVP2.Components.Custom;
public abstract class FarmComponentBase : ComponentBase
{
    [CascadingParameter]
    protected MainFarmContainer? Farm { get; set; }
    protected CropManager CropManager => Farm!.CropManager;
    protected TreeManager TreeManager => Farm!.TreeManager;
    protected WorkshopManager WorkshopManager => Farm!.WorkshopManager;
    protected AnimalManager AnimalManager => Farm!.AnimalManager;
    public WorksiteManager WorksiteManager => Farm!.WorksiteManager;
    protected InventoryManager InventoryManager => Farm!.InventoryManager;
    protected UpgradeManager UpgradeManager => Farm!.UpgradeManager;
    protected ProgressionManager ProgressionManager => Farm!.ProgressionManager;
    protected StoreManager StoreManager => Farm!.StoreManager;
}