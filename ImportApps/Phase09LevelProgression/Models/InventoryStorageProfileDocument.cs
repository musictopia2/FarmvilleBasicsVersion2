namespace Phase09LevelProgression.Models;
public class InventoryStorageProfileDocument
{
    required public FarmKey Farm { get; set; }
    //suggested using size and not limit
    
    public int BarnLevel { get; set; }
    public int SiloLevel { get; set; }
    //suggested keeping these (for compatibility plus so when doing lookups, easier to do).
    public int BarnSize { get; set; }
    public int SiloSize { get; set; }
}