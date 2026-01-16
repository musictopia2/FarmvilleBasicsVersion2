namespace Phase14MVP2.Services.Worksites;
public class WorksiteManager(
    InventoryManager inventory,
    IBaseBalanceProvider baseBalanceProvider,
    ItemRegistry itemRegistry
    )
{
    private bool _canAutomateCollection;
    private IWorksiteCollectionPolicy? _worksiteCollectionPolicy;
    private IWorksiteRepository _worksiteRepository = null!;
    private IWorkerRepository _workerRepository = null!;
    private readonly BasicList<WorksiteInstance> _worksites = [];
    private BasicList<UnlockModel> _workerStates = [];
    private BasicList<WorkerRecipe> _allWorkers = [];
    private bool _needsSaving;
    private DateTime _lastSave = DateTime.MinValue;
    private readonly Lock _lock = new();

    public BasicList<string> GetAllUnlockedWorkers()
    {
        return _workerStates.Where(x => x.Unlocked).Select(x => x.Name).ToBasicList();
    }

    public BasicList<WorkerRecipe> GetUnlockedWorkers(string location)
    {
        var unlockedNames = _workerStates.Where(x => x.Unlocked).Select(x => x.Name).ToBasicList();
        BasicList<WorkerRecipe> output = [];

        foreach (var unlockedName in unlockedNames)
        {
            var recipe = _allWorkers.Single(x => x.WorkerName == unlockedName);
            if (recipe.WorkerStatus == EnumWorkerStatus.Selected)
            {
                output.Add(recipe);
            }
            else if (recipe.WorkerStatus == EnumWorkerStatus.None)
            {
                output.Add(recipe);
            }
            else if (recipe.CurrentLocation == location)
            {
                output.Add(recipe);
            }
        }
        return output;
    }
    public async Task UnlockWorkerPaidForAsync(StoreItemRowModel store)
    {
        if (store.Category != EnumCatalogCategory.Worker)
        {
            throw new CustomBasicException("Only workers can be paid for");
        }
        var item = _workerStates.First(x => x.Name == store.TargetName && x.Unlocked == false);
        item.Unlocked = true;
        await _workerRepository.SaveAsync(_workerStates);
    }
    public void UnlockWorksitePaidFor(StoreItemRowModel store)
    {
        if (store.Category != EnumCatalogCategory.Worksite)
        {
            throw new CustomBasicException("Only worksites can be paid for");
        }
        var item = _worksites.First(x => x.Unlocked == false && x.Location == store.TargetName);
        item.Unlocked = true;
        _needsSaving = true;
    }

    public async Task ApplyWorkerProgressionUnlocksFromLevelsAsync(BasicList<CatalogOfferModel> offers, int level)
    {
        //only unlock current level.
        var item = offers.SingleOrDefault(x => x.LevelRequired == level);
        if (item is null)
        {
            return;
        }

        var worker = _workerStates.Single(x => x.Name == item.TargetName);
        worker.Unlocked = true;
        await _workerRepository.SaveAsync(_workerStates);

    }

    public int TotalWorkersAllowed(string location)
    {
        var site = GetWorksiteByLocation(location);
        return site.MaximumWorkers;
    }
    public string? GetPossibleWorksiteForItem(string name) => _worksites.SingleOrDefault(x => x.HasRecipe(name))?.Location;
    public void ApplyWorksiteProgressionUnlocksFromLevels(BasicList<CatalogOfferModel> offers, int level)
    {
        //only unlock current level.
        var item = offers.SingleOrDefault(x => x.LevelRequired == level);
        if (item is null)
        {
            return;
        }
        var instance = _worksites.Single(x => x.Location == item.TargetName);
        instance.Unlocked = true;
        _needsSaving = true;
    }


    private WorksiteInstance GetWorksiteByLocation(string location)
    {
        var worksite = _worksites.SingleOrDefault(t => t.Location == location) ?? throw new CustomBasicException($"Worksite with location {location} not found.");
        return worksite;
    }
    
    public void AddWorker(string location, WorkerRecipe worker)
    {
        WorksiteInstance instance = GetWorksiteByLocation(location);
        instance.AddWorker(worker);
        _needsSaving = true;
    }
    public void RemoveWorker(string location, WorkerRecipe worker)
    {
        WorksiteInstance instance = GetWorksiteByLocation(location);
        instance.RemoveWorker(worker);
        _needsSaving = true;
    }


    public bool CanResetToFocused(string location)
    {
        WorksiteInstance instance = GetWorksiteByLocation(location);
        return instance.CanResetToFocused;
    }
    public void ResetToFocused(string location)
    {
        WorksiteInstance instance = GetWorksiteByLocation(location);
        instance.ResetToFocused();
        _needsSaving = true;
    }
    public bool CanStartJob(string location)
    {
        WorksiteInstance instance = GetWorksiteByLocation(location);
        return instance.CanStartJob(inventory);
    }
    public bool NeedsAutomateError(string location)
    {
        WorksiteInstance instance = GetWorksiteByLocation(location);
        return instance.NeedsAutomateError();
    }
    public void StartJob(string location)
    {
        WorksiteInstance instance = GetWorksiteByLocation(location);
        instance.StartJob(inventory);
        _needsSaving = true;
    }
    public bool CanCollectRewards(string location)
    {
        WorksiteInstance instance = GetWorksiteByLocation(location);
        //later can have other reasons you cannot collect rewards (later).
        return instance.CanCollectRewards;
    }
    public bool CanCollectRewardsWithLimits(string location)
    {
        var list = GetRewards(location);
        return inventory.CanAcceptRewards(list);
    }
    public BasicList<ItemAmount> GetRewards(string location)
    {
        WorksiteInstance instance = GetWorksiteByLocation(location);
        return instance.GetRewards();
    }
    //public bool CanCollectRewards(string location)
    //{

    //}
    public void CollectAllRewards(string location, BasicList<ItemAmount> rewards)
    {
        WorksiteInstance instance = GetWorksiteByLocation(location);
        CollectAllRewards(instance, rewards);
    }

    public void CollectSpecificReward(string location, ItemAmount reward)
    {
        WorksiteInstance instance = GetWorksiteByLocation(location);
        instance.CollectSpecificReward(reward);
        inventory.Add(reward.Item, reward.Amount);
        _needsSaving = true;
    }

    //give a person a choice (so if they had ui that forces in order, can do).
    public void CollectFirstReward(string location)
    {
        WorksiteInstance instance = GetWorksiteByLocation(location);
        ItemAmount reward = instance.GetFirstReward;
        inventory.Add(reward.Item, reward.Amount);
        _needsSaving = true;
    }

    private void CollectAllRewards(WorksiteInstance instance, BasicList<ItemAmount> rewards)
    {
        rewards.ForEach(reward =>
        {
            inventory.Add(reward.Item, reward.Amount);
        });
        instance.CollectAllRewards();
        _needsSaving = true;
    }
    public BasicList<WorkerRecipe> GetWorkers(string location)
    {
        WorksiteInstance instance = GetWorksiteByLocation(location);
        return instance.Workers;
    }
    public EnumWorksiteState GetStatus(string location)
    {
        WorksiteInstance instance = GetWorksiteByLocation(location);
        return instance.Status;
    }
    public string GetDurationText(string location)
    {
        WorksiteInstance instance = GetWorksiteByLocation(location);
        return $"{instance.StartText} ({instance.EffectiveDuration.GetTimeString})"; //i think this is what is needed (?)
    }
    public string GetProgressText(string location)
    {
        WorksiteInstance instance = GetWorksiteByLocation(location);
        if (instance.ReadyTime is null)
        {
            return "Finished";
        }
        try
        {
            return $"Come back in {instance.ReadyTime.Value.GetTimeString}";
        }
        catch (Exception)
        {
            return "Finished";
        }
        
    }
    public BasicList<WorksiteRewardPreview> GetPreview(string location)
    {
        WorksiteInstance instance = GetWorksiteByLocation(location);
        return instance.GetPreview();
    }
    public BasicList<ItemAmount> SuppliesNeeded(string location)
    {
        WorksiteInstance instance = GetWorksiteByLocation(location);
        return instance.SuppliesNeeded;
    }
    public BasicList<string> GetUnlockedWorksites()
    {
        BasicList<string> output = [];
        _worksites.ForConditionalItems(x => x.Unlocked, t =>
        {
            output.Add(t.Location);
        });
        return output;
    }
    
    
    public async Task<bool> CanAutomateCollectionAsync()
    {
        _canAutomateCollection = await _worksiteCollectionPolicy!.CollectAllAsync();
        return _canAutomateCollection;
    }
    public async Task SetStyleContextAsync(WorksiteServicesContext worksiteContext,
        WorkerServicesContext workerContext,
        FarmKey farm
        )
    {
        if (_worksiteRepository != null)
        {
            throw new InvalidOperationException("Persistance Already set");
        }
        _worksiteRepository = worksiteContext.WorksiteRepository;
        _workerRepository = workerContext.WorkerRepository;
        BasicList<WorksiteRecipe> recipes = await worksiteContext.WorksiteRegistry.GetWorksitesAsync();
        foreach (var item in recipes)
        {
            foreach (var temp in item.BaselineBenefits)
            {
                EnumInventoryStorageCategory category;
                if (temp.Optional)
                {
                    category = EnumInventoryStorageCategory.None;
                }
                else
                {
                    category = EnumInventoryStorageCategory.Barn;
                }
                itemRegistry.Register(new(temp.Item, category, EnumInventoryItemCategory.Worksites));
            }
        }
        _worksiteCollectionPolicy = worksiteContext.WorksiteCollectionPolicy;
        _canAutomateCollection = await _worksiteCollectionPolicy!.CollectAllAsync();
        _worksites.Clear();
        _workerStates = await workerContext.WorkerRepository.LoadAsync();
        var ours = await worksiteContext.WorksiteRepository.LoadAsync();
        _allWorkers = await workerContext.WorkerRegistry.GetWorkersAsync();
        BaseBalanceProfile profile = await baseBalanceProvider.GetBaseBalanceAsync(farm);
        double offset = profile.WorksiteTimeMultiplier;

        foreach (var item in ours)
        {
            WorksiteRecipe recipe = recipes.Single(x => x.Location == item.Name);
            WorksiteInstance instance = new(recipe, offset, _allWorkers, _workerStates);
            instance.Load(item);
            foreach (var tempWorker in item.Workers)
            {
                WorkerRecipe reals = _allWorkers.Single(x => x.WorkerName == tempWorker.WorkerName);
                reals.WorkerStatus = tempWorker.WorkerStatus;
                reals.CurrentLocation = tempWorker.CurrentLocation;
                instance.AddWorkerAfterLoading(reals);
            }
            _worksites.Add(instance);
        }
    }
    public void StoreRewards(string location, BasicList<ItemAmount> rewards)
    {
        var worksite = _worksites.Single(x => x.Location == location);
        worksite.StoreRewards(rewards);
        _needsSaving = true;
    }

    // Tick method called by game timer
    public async Task UpdateTickAsync()
    {
        _worksites.ForConditionalItems(x => x.Unlocked && x.Status != EnumWorksiteState.None, worksite =>
        {
            worksite.UpdateTick();
            if (worksite.NeedsSaving)
            {
                _needsSaving = true;
            }
            //may be automated now.
            if (worksite.Status == EnumWorksiteState.Collecting && _canAutomateCollection == true)
            {
                var rewards = worksite.GetRewards();
                if (inventory.CanAcceptRewards(rewards))
                {
                    CollectAllRewards(worksite, rewards);
                }
            }
        });
        await SaveWorksitesAsync();
    }

    private async Task SaveWorksitesAsync()
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
            BasicList<WorksiteAutoResumeModel> list = _worksites
             .Select(worksite => worksite.GetWorksiteForSaving)
             .ToBasicList();
            await _worksiteRepository.SaveAsync(list);
        }
    }
}