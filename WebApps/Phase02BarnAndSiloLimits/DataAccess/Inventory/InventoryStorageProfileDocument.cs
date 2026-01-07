namespace Phase02BarnAndSiloLimits.DataAccess.Inventory;
public class InventoryStorageProfileDocument
{
    required public FarmKey Farm { get; set; }
    //suggested using size and not limit
    public int BarnSize { get; set; }
    public int SiloSize { get; set; }
}