namespace Phase08UpgradeWorkshopCapacity.Services.Upgrades;
public class UpgradeServicesContext
{
    required public IInventoryStorageUpgradePlanProvider InventoryStorageUpgradePlanProvider { get; init; }
    //this is everything that needs to be resolved so the upgrade manager can do its job.

}