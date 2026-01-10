namespace Phase07EarnCoinFromQuests.DataAccess.Upgrades;
public class UpgradeFactory : IUpgradeFactory
{
    UpgradeServicesContext IUpgradeFactory.GetUpgradeServices(FarmKey farm)
    {
        return new UpgradeServicesContext()
        {
            InventoryStorageUpgradePlanProvider = new InventoryStorageUpgradePlanDatabase()
        };
    }
}