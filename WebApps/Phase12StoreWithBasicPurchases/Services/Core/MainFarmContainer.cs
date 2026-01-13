namespace Phase12StoreWithBasicPurchases.Services.Core;
public class MainFarmContainer
{
    required public CropManager CropManager { get; set; }
    required public TreeManager TreeManager { get; set; }
    required public InventoryManager InventoryManager { get; set; }
    required public AnimalManager AnimalManager { get; set; }
    required public WorkshopManager WorkshopManager { get; set; }
    required public WorksiteManager WorksiteManager { get; set; }
    required public QuestManager QuestManager { get; set; }
    required public UpgradeManager UpgradeManager { get; set; }
    required public ProgressionManager ProgressionManager { get; set; }
}