namespace Phase08UpgradeWorkshopCapacity.Services.Upgrades;
public interface IInventoryStorageUpgradePlanProvider
{
    Task<InventoryStorageUpgradePlanModel> GetPlanAsync(FarmKey farm);
}