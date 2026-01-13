namespace Phase12StoreWithBasicPurchases.DataAccess.Inventory;
public class InventoryStockDocument
{
    required public FarmKey Farm { get; set; }
    public Dictionary<string, int> List { get; set; } = [];
}