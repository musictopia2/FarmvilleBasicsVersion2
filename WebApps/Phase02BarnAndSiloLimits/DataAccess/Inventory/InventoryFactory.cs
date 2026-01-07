namespace Phase02BarnAndSiloLimits.DataAccess.Inventory;
public class InventoryFactory : IInventoryFactory
{
    IInventoryProfile IInventoryFactory.GetInventoryProfile(FarmKey farm)
    {
        return new InventoryStorageProfileDatabase();
    }

    IInventoryRepository IInventoryFactory.GetInventoryServices(FarmKey farm)
    {
        return new InventoryStockDatabase();
    }
}