namespace Phase10ProgressionUnlocks.Models;
public class InventoryStockDocument
{
    required public FarmKey Farm { get; set; }
    public Dictionary<string, int> List { get; set; } = [];
}