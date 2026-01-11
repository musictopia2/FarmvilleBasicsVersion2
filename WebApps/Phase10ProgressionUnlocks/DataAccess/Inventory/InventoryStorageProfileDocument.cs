namespace Phase10ProgressionUnlocks.DataAccess.Inventory;
public class InventoryStorageProfileDocument
{
    required public FarmKey Farm { get; set; }
    //suggested using size and not limit
    public int BarnSize { get; set; }
    public int SiloSize { get; set; }
    public int BarnLevel { get; set; }
    public int SiloLevel { get; set; }
}