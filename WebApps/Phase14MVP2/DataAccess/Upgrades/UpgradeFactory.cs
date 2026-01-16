namespace Phase14MVP2.DataAccess.Upgrades;
public class UpgradeFactory : IUpgradeFactory
{
    UpgradeServicesContext IUpgradeFactory.GetUpgradeServices(FarmKey farm)
    {
        return new UpgradeServicesContext()
        {
            InventoryStorageUpgradePlanProvider = new InventoryStorageUpgradePlanDatabase(),
            WorkshopCapacityUpgradePlanProvider = new WorkshopCapacityUpgradePlanDatabase(),
        };
    }
}