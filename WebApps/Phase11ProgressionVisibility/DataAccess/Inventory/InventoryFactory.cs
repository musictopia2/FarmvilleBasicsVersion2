namespace Phase11ProgressionVisibility.DataAccess.Inventory;
public class InventoryFactory : IInventoryFactory
{
    IInventoryProfile IInventoryFactory.GetInventoryProfile(FarmKey farm)
    {
        return new InventoryStorageProfileDatabase(farm);
    }

    IInventoryRepository IInventoryFactory.GetInventoryServices(FarmKey farm)
    {
        return new InventoryStockDatabase();
    }
}