namespace Phase09LevelProgression.DataAccess.Inventory;
public class InventoryStockDocument
{
    required public FarmKey Farm { get; set; }
    public Dictionary<string, int> List { get; set; } = [];
}