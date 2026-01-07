using Phase02BarnAndSiloLimits.Services.Inventory;

namespace Phase02BarnAndSiloLimits.Components.Custom;
public abstract class FarmComponentBase : ComponentBase
{
    [CascadingParameter]
    protected MainFarmContainer? Farm { get; set; }
    protected CropManager CropManager => Farm!.CropManager;
    protected TreeManager TreeManager => Farm!.TreeManager;
    protected WorkshopManager WorkshopManager => Farm!.WorkshopManager;
    protected AnimalManager AnimalManager => Farm!.AnimalManager;
    public WorksiteManager WorksiteManager => Farm!.WorksiteManager;
    protected InventoryManager Inventory => Farm!.InventoryManager;
}