using Phase01IntroduceBarnAndSilo.Services.Core;

namespace Phase01IntroduceBarnAndSilo.DataAccess.Inventory;
public class InventoryDocument
{
    required public FarmKey Farm { get; set; }
    public Dictionary<string, int> List { get; set; } = [];
}