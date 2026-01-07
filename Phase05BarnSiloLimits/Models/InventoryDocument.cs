namespace Phase05BarnSiloLimits.Models;
public class InventoryDocument
{
    required public FarmKey Farm { get; set; }
    public Dictionary<string, int> List { get; set; } = [];
}