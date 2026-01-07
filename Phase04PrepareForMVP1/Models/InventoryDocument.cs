namespace Phase04PrepareForMVP1.Models;
public class InventoryDocument
{
    required public FarmKey Farm { get; set; }
    public Dictionary<string, int> List { get; set; } = [];
}