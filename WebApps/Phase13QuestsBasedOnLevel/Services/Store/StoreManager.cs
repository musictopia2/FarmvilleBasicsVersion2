namespace Phase13QuestsBasedOnLevel.Services.Store;
public class StoreManager(IFarmProgressionReadOnly levelProgression,
    TreeManager treeManager,
    AnimalManager animalManager,
    WorkshopManager workshopManager,
    WorksiteManager worksiteManager,
    CatalogManager catalogManager,
    InventoryManager inventoryManager
    )
{
    private int _currentLevel;
    private bool _completedGame;
    private EnumCatalogCategory _catalogCategory;
    private IStoreUiStateRepository _storeUiStateRepository = null!;
    private BasicList<CatalogOfferModel> _treesOffers = [];
    private BasicList<CatalogOfferModel> _animalOffers = [];
    private BasicList<CatalogOfferModel> _workshopOffers = [];
    private BasicList<CatalogOfferModel> _worksiteOffers = [];
    private BasicList<CatalogOfferModel> _workerOffers = [];
    public EnumCatalogCategory CurrentCategory => _catalogCategory;
    public async Task SetProgressionStyleContextAsync(StoreServicesContext context)
    {
        _catalogCategory = await context.UiStateRepository.LoadAsync();
        _storeUiStateRepository = context.UiStateRepository;
        _treesOffers = catalogManager.GetAllOffers(EnumCatalogCategory.Tree);
        _animalOffers = catalogManager.GetAllOffers(EnumCatalogCategory.Animal);
        _workshopOffers = catalogManager.GetAllOffers(EnumCatalogCategory.Workshop);
        _worksiteOffers = catalogManager.GetAllOffers(EnumCatalogCategory.Worksite);
        _workerOffers = catalogManager.GetAllOffers(EnumCatalogCategory.Worker);
        levelProgression.Changed += Refresh;
        Refresh();
    }
    public async Task ChoseNewCategoryAsync(EnumCatalogCategory category)
    {
        _catalogCategory = category;
        await _storeUiStateRepository.SaveAsync(_catalogCategory);
    }
    public bool CanAfford(StoreItemRowModel store) => inventoryManager.Has(store.Costs);
    public BasicList<StoreItemRowModel> GetStoreItems()
    {
        if (_completedGame)
        {
            return [];
        }
        if (_catalogCategory == EnumCatalogCategory.Tree)
        {
            return GetTrees();
        }
        if (_catalogCategory == EnumCatalogCategory.Animal)
        {
            return GetAnimals();
        }
        if (_catalogCategory == EnumCatalogCategory.Workshop)
        {
            return GetWorkshops();
        }
        if (_catalogCategory == EnumCatalogCategory.Worker)
        {
            return GetWorkers();
        }
        if (_catalogCategory == EnumCatalogCategory.Worksite)
        {
            return GetWorksites();
        }
        throw new CustomBasicException("Not supported yet");
    }
    private BasicList<StoreItemRowModel> GetAnimals()
    {
        var offers = _animalOffers;
        if (offers is null || offers.Count == 0)
        {
            return [];
        }
        var ownedLookup = BuildOwnedLookup(animalManager.GetUnlockedAnimals, x => x.Name);
        return BuildRowsFromTieredOffers(
            offers,
            targetName => ownedLookup.TryGetValue(targetName, out int c) ? c : 0,
            EnumCatalogCategory.Animal
        );
    }
    private BasicList<StoreItemRowModel> GetTrees()
    {
        var offers = _treesOffers;
        if (offers is null || offers.Count == 0)
        {
            return [];
        }

        // Build owned counts from the tree manager (Name matches CatalogOfferModel.TargetName)
        var ownedLookup = treeManager.GetUnlockedTrees
            .GroupBy(t => t.TreeName)
            .ToDictionary(g => g.Key, g => g.Count());

        return BuildRowsFromTieredOffers(
            offers,
            targetName => ownedLookup.TryGetValue(targetName, out int c) ? c : 0,
            EnumCatalogCategory.Tree
        );
    }
    private BasicList<StoreItemRowModel> GetWorkshops()
    {
        var offers = _workshopOffers;
        if (offers is null || offers.Count == 0)
        {
            return [];
        }
        var ownedLookup = workshopManager.GetUnlockedWorkshops
            .GroupBy(t => t.Name)
            .ToDictionary(g => g.Key, g => g.Count());
        return BuildRowsFromTieredOffers(
            offers,
            targetName => ownedLookup.TryGetValue(targetName, out int c) ? c : 0,
            EnumCatalogCategory.Workshop
        );
    }

    private BasicList<StoreItemRowModel> GetWorksites()
    {
        var offers = _worksiteOffers;
        if (offers is null || offers.Count == 0)
        {
            return [];
        }
        var ownedLookup = worksiteManager.GetUnlockedWorksites()
            .GroupBy(t => t)
            .ToDictionary(g => g.Key, g => g.Count());
        return BuildRowsFromTieredOffers(
            offers,
            targetName => ownedLookup.TryGetValue(targetName, out int c) ? c : 0,
            EnumCatalogCategory.Worksite
        );
    }
    private BasicList<StoreItemRowModel> GetWorkers()
    {
        var offers = _workerOffers;
        if (offers is null || offers.Count == 0)
        {
            return [];
        }
        var ownedLookup = worksiteManager.GetAllUnlockedWorkers()
            .GroupBy(t => t)
            .ToDictionary(g => g.Key, g => g.Count());
        return BuildRowsFromTieredOffers(
            offers,
            targetName => ownedLookup.TryGetValue(targetName, out int c) ? c : 0,
            EnumCatalogCategory.Worker
        );
    }
    private static Dictionary<string, int> BuildOwnedLookup<T>(
        IEnumerable<T> items,
        Func<T, string> keySelector)
    {
        return items
            .GroupBy(keySelector)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    private BasicList<StoreItemRowModel> BuildRowsFromTieredOffers(
    BasicList<CatalogOfferModel> offers,
    Func<string, int> ownedCountProvider,
    EnumCatalogCategory category)
    {
        if (offers is null || offers.Count == 0)
        {
            return [];
        }

        // Preserve catalog order: the first time a TargetName appears in offers
        // (this matches "order of the catalog" even after grouping)
        var firstIndex = new Dictionary<string, int>();
        for (int i = 0; i < offers.Count; i++)
        {
            var name = offers[i].TargetName;
            if (!firstIndex.ContainsKey(name))
            {
                firstIndex[name] = i;
            }
        }

        BasicList<StoreItemRowModel> rows = [];

        foreach (var group in offers.GroupBy(x => x.TargetName))
        {
            string targetName = group.Key;

            var tiers = group
                .OrderBy(x => x.LevelRequired)
                .ToBasicList();

            int totalPossible = tiers.Count;
            int ownedCount = ownedCountProvider(targetName);

            bool isMaxedOut = ownedCount >= totalPossible;

            if (isMaxedOut)
            {
                rows.Add(new StoreItemRowModel
                {
                    TargetName = targetName,
                    TotalPossible = totalPossible,
                    OwnedCount = ownedCount,
                    LevelRequired = 0,
                    Costs = [],
                    IsLocked = false,
                    IsMaxedOut = true,
                    Category = category
                });
                continue;
            }

            // Next tier is based on how many already owned
            var nextTier = tiers[ownedCount];
            bool isLocked = _currentLevel < nextTier.LevelRequired;

            rows.Add(new StoreItemRowModel
            {
                TargetName = targetName,
                TotalPossible = totalPossible,
                OwnedCount = ownedCount,
                LevelRequired = nextTier.LevelRequired,
                Costs = nextTier.Costs,
                IsLocked = isLocked,
                IsMaxedOut = false,
                Category = category
            });
        }
        return rows;
        //return rows.OrderBy(x => x.LevelRequired).ToBasicList();
            
    }
    private void Refresh()
    {
        _currentLevel = levelProgression.CurrentLevel;
        _completedGame = levelProgression.CompletedGame;
    }
    public bool CanAcquire(StoreItemRowModel store)
    {
        if (inventoryManager.Has(store.Costs) == false)
        {
            return false;
        }
        if (store.IsMaxedOut)
        {
            return false;
        }
        if (store.LevelRequired > _currentLevel)
        {
            return false;
        }
        if (store.IsFree)
        {
            return false; //because the level progression should had handled this.
        }
        return true; //i think.
    }
    public async Task AcquireAsync(StoreItemRowModel store)
    {
        if (CanAcquire(store) == false)
        {
            throw new CustomBasicException("Unable to acquire this.  Should had called CanAcquire first");
        }
        //now needs to actually unlock the items.
        if (store.Category == EnumCatalogCategory.Tree)
        {
            treeManager.UnlockTreePaidFor(store);
            FinishAcquiring(store);
            return;
        }
        if (store.Category == EnumCatalogCategory.Animal)
        {
            //if rentals are needed, will rethink.
            animalManager.UnlockAnimalPaidFor(store);
            FinishAcquiring(store);
            return;
        }
        if (store.Category == EnumCatalogCategory.Workshop)
        {
            workshopManager.UnlockWorkshopPaidFor(store);
            FinishAcquiring(store);
            return;
        }
        if (store.Category == EnumCatalogCategory.Worksite)
        {
            worksiteManager.UnlockWorksitePaidFor(store);
            FinishAcquiring(store);
            return;
        }
        if (store.Category == EnumCatalogCategory.Worker)
        {
            await worksiteManager.UnlockWorkerPaidForAsync(store);
            FinishAcquiring(store);
        }
    }

    private void FinishAcquiring(StoreItemRowModel store)
    {
        inventoryManager.Consume(store.Costs);
    }



}
