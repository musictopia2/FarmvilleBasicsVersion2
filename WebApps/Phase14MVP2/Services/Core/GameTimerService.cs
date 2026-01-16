namespace Phase14MVP2.Services.Core;
public class GameTimerService(IStartFarmRegistry farmRegistry,
    IInventoryRepository inventoryRepository,
    IBaseBalanceProvider baseBalanceProvider,
    GameRegistry gameRegistry, IServiceProvider sp) : BackgroundService
{
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        BasicList<FarmKey> firsts = await farmRegistry.GetFarmsAsync();
        foreach (var farm in firsts)
        {
            //could create a factory to produce this.
            //here will need to figure out the interface for this.

            ItemRegistry itemRegistry = new();
            InventoryManager inventory = new(farm, inventoryRepository, itemRegistry);
            IInventoryFactory starts = sp.GetRequiredService<IInventoryFactory>();
            ICropFactory cropFactory = sp.GetRequiredService<ICropFactory>();
            ITreeFactory treeFactory = sp.GetRequiredService<ITreeFactory>();
            IAnimalFactory animalFactory = sp.GetRequiredService<IAnimalFactory>();
            IWorkshopFactory workshopFactory = sp.GetRequiredService<IWorkshopFactory>();
            IWorksiteFactory worksiteFactory = sp.GetRequiredService<IWorksiteFactory>();
            IWorkerFactory workerFactory = sp.GetRequiredService<IWorkerFactory>();
            IQuestFactory questFactory = sp.GetRequiredService<IQuestFactory>();
            IUpgradeFactory upgradeFactory = sp.GetRequiredService<IUpgradeFactory>();
            IProgressionFactory progressionFactory = sp.GetRequiredService<IProgressionFactory>();
            ICatalogFactory catalogFactory = sp.GetRequiredService<ICatalogFactory>();
            IStoreFactory storeFactory = sp.GetRequiredService<IStoreFactory>();
            IItemFactory itemFactory = sp.GetRequiredService<IItemFactory>();
            CropManager cropManager = new(inventory, baseBalanceProvider, itemRegistry);
            TreeManager treeManager = new(inventory, baseBalanceProvider, itemRegistry);
            AnimalManager animalManager = new(inventory, baseBalanceProvider, itemRegistry);
            WorkshopManager workshopManager = new(inventory, baseBalanceProvider, itemRegistry);
            WorksiteManager worksiteManager = new(inventory, baseBalanceProvider, itemRegistry);
            ItemManager itemManager = new();
            CatalogManager catalogManager = new();
            var profile = starts.GetInventoryProfile(farm);
            UpgradeManager upgradeManager = new(inventory, profile, workshopManager);
            ProgressionManager progressionManager = new(inventory, cropManager,
                animalManager, treeManager, workshopManager, worksiteManager, catalogManager);
            StoreManager storeManager = new(progressionManager, treeManager,
                animalManager, workshopManager, worksiteManager,
                catalogManager, inventory
                );
            QuestManager questManager = new(inventory, itemManager, progressionManager);
            IGameTimer timer = new BasicGameState(
                inventory, starts,
                cropFactory, treeFactory, animalFactory, workshopFactory,
                worksiteFactory, workerFactory, questFactory,
                upgradeFactory, progressionFactory, catalogFactory, storeFactory, itemFactory,
                cropManager, treeManager, animalManager,
                workshopManager, worksiteManager, questManager,
                upgradeManager, progressionManager, catalogManager, storeManager, itemManager
                );
            await gameRegistry.InitializeFarmAsync(timer, farm);
        }
        await base.StartAsync(cancellationToken);
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await gameRegistry.TickAsync();
            }
            catch
            {
                // ignore
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}