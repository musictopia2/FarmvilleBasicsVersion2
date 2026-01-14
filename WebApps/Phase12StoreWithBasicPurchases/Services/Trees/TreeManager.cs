namespace Phase12StoreWithBasicPurchases.Services.Trees;
public class TreeManager(InventoryManager inventory,
    IBaseBalanceProvider baseBalanceProvider,
    ItemRegistry itemRegistry
    )
{
    private ITreesCollecting? _treeCollecting;
    private ITreeRepository _treeRepository = null!;
    private BasicList<TreeRecipe> _recipes = [];
    private readonly Lock _lock = new();
    private bool _needsSaving;
    private DateTime _lastSave = DateTime.MinValue;
    private readonly BasicList<TreeInstance> _trees = [];
    private bool _collectAll;
    // Public read-only summaries for the UI
    public BasicList<TreeView> GetUnlockedTrees
    {
        get
        {
            BasicList<TreeView> output = [];
            _trees.ForConditionalItems(x => x.Unlocked, t =>
            {
                TreeView summary = new()
                {
                    Id = t.Id,
                    ItemName = t.Name,
                    TreeName = t.TreeName
                };
                output.Add(summary);
            });
            return output;
        }
    }

    public void UnlockTreePaidFor(StoreItemRowModel store)
    {
        if (store.Category != EnumCatalogCategory.Tree)
        {
            throw new CustomBasicException("Only trees can be paid for");
        }
        var instance = _trees.First(x => x.TreeName == store.TargetName && x.Unlocked == false);
        instance.Unlocked = true;
        _needsSaving = true;
    }

    public void ApplyTreeUnlocksOnLevels(BasicList<CatalogOfferModel> offers, int level) //actually since this is from leveling, has to apply t
    {
        //only unlock current level.
        var item = offers.FirstOrDefault(x => x.LevelRequired == level);
        if (item is null)
        {
            return;
        }
        var instance = _trees.First(x => x.TreeName == item.TargetName && x.Unlocked == false);
        instance.Unlocked = true;
        _needsSaving = true;

    }
    // Private helper to find tree by Id
    public int GetProduceAmount(TreeView tree)
    {
        CustomBasicException.ThrowIfNull(tree); //still needs to pass since i may use in future.
        CustomBasicException.ThrowIfNull(_treeCollecting);
        return _treeCollecting.TreesCollectedAtTime; // for now
    }
    private TreeInstance GetTreeById(Guid id)
    {
        var tree = _trees.SingleOrDefault(t => t.Id == id) ?? throw new CustomBasicException($"Tree with Id {id} not found.");
        return tree;
    }
    private TreeInstance GetTreeById(TreeView id) => GetTreeById(id.Id);

    public bool HasTrees(string name) => _recipes.Exists(x => x.Item == name);
    public TimeSpan TreeDuration(TreeView id) => GetTreeById(id).BaseTime;

    // Methods for UI to query dynamic state
    public int TreesReady(TreeView id) => GetTreeById(id).TreesReady;
    public string GetTreeName(TreeView id) => GetTreeById(id).Name;
    public string TimeLeftForResult(TreeView id) => GetTreeById(id).ReadyTime!.Value!.GetTimeString;
    public EnumTreeState GetTreeState(TreeView id) => GetTreeById(id).State;
    //this is when you collect only one item.


    public bool CanCollectFromTree(TreeView id)
    {
        TreeInstance instance = GetTreeById(id);
        int amount = GetCollectAmount(instance);
        return inventory.CanAdd(instance.Name, amount);
    }
    private int GetCollectAmount(TreeInstance instance)
    {
        //int maxs;
        if (_collectAll)
        {
            return instance.TreesReady;
        }
        return 1;
    }
    public void CollectFromTree(TreeView id)
    {
        if (CanCollectFromTree(id) == false)
        {
            throw new CustomBasicException("Unable to collect from tree.  Should had used CanCollectFromTree");
        }
        TreeInstance instance = GetTreeById(id);
        int maxs = GetCollectAmount(instance);
        maxs.Times(x =>
        {
            instance.CollectTree();
        });
        inventory.Add(instance.Name, maxs);
        _needsSaving = true;
    }

    public async Task SetStyleContextAsync(TreeServicesContext context, FarmKey farm)
    {
        //_treeGatheringPolicy = context.TreeGatheringPolicy;
        _collectAll = await context.TreeGatheringPolicy.CollectAllAsync();
        //if this changes, rethink later.
        if (_treeRepository != null)
        {
            throw new InvalidOperationException("Repository Already set");
        }
        _treeRepository = context.TreeRepository;
        _recipes = await context.TreeRegistry.GetTreesAsync();
        foreach (var item in _recipes)
        {
            itemRegistry.Register(new(item.Item, EnumInventoryStorageCategory.Silo, EnumInventoryItemCategory.Trees));
        }
        var ours = await context.TreeRepository.LoadAsync();
        _trees.Clear();
        _treeCollecting = context.TreesCollecting;
        BaseBalanceProfile profile = await baseBalanceProvider.GetBaseBalanceAsync(farm);
        double offset = profile.TreeTimeMultiplier;
        foreach (var item in ours)
        {
            TreeRecipe recipe = _recipes.Single(x => x.TreeName == item.TreeName);
            TreeInstance tree = new(recipe, _treeCollecting, offset);
            tree.Load(item);
            _trees.Add(tree);
        }
    }
    // Tick method called by game timer
    public async Task UpdateTickAsync()
    {
        _trees.ForConditionalItems(x => x.Unlocked, tree =>
        {
            tree.UpdateTick();
            if (tree.NeedsToSave)
            {
                _needsSaving = true;
            }
        });
        await SaveTreesAsync();
    }
    private async Task SaveTreesAsync()
    {
        bool doSave = false;
        lock (_lock)
        {
            if (_needsSaving && DateTime.Now - _lastSave > GameRegistry.SaveThrottle)
            {
                _needsSaving = false;
                doSave = true;
                _lastSave = DateTime.Now;
            }
        }
        if (doSave)
        {
            BasicList<TreeAutoResumeModel> list = _trees
             .Select(tree => tree.GetTreeForSaving)
             .ToBasicList();
            await _treeRepository.SaveAsync(list);
        }
    }
}