namespace Phase13QuestsBasedOnLevel.Services.Upgrades;
public interface IInventoryStorageUpgradePlanProvider
{
    Task<InventoryStorageUpgradePlanModel> GetPlanAsync(FarmKey farm);
}