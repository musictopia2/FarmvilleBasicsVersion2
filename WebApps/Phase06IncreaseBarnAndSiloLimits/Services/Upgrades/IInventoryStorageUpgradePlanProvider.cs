namespace Phase06IncreaseBarnAndSiloLimits.Services.Upgrades;
public interface IInventoryStorageUpgradePlanProvider
{
    Task<InventoryStorageUpgradePlanModel> GetPlanAsync(FarmKey farm);
}