namespace Phase01IntroduceBarnAndSilo.Services.Workshops;
public class WorkshopManager(InventoryManager inventory,
    IBaseBalanceProvider baseBalanceProvider,
    ItemRegistry itemRegistry
    )
{
    private IWorkshopProgressionPolicy? _workshopProgressionPolicy;
    private IWorkshopCollectionPolicy? _workshopCollectionPolicy;
    private IWorkshopPersistence _workshopPersistence = null!;
    private bool _automateCollection;
    private readonly BasicList<WorkshopInstance> _workshops = [];
    private BasicList<WorkshopRecipe> _recipes = [];
    public event Action? OnWorkshopsUpdated;
    private readonly Lock _lock = new();
    private bool _needsSaving;
    private DateTime _lastSave = DateTime.MinValue;
    private double _multiplier;
    private WorkshopInstance GetWorkshopById(Guid id)
    {
        var workshop = _workshops.SingleOrDefault(t => t.Id == id) ?? throw new CustomBasicException($"Workshop with Id {id} not found.");
        return workshop;
    }
    private WorkshopInstance GetWorkshopById(WorkshopView id) => GetWorkshopById(id.Id);
    public BasicList<WorkshopView> GetUnlockedWorkshops
    {
        get
        {
            lock (_lock)
            {
                BasicList<WorkshopView> output = [];
                _workshops.ForConditionalItems(x => x.Unlocked, t =>
                {
                    WorkshopView summary = new()
                    {
                        Id = t.Id,
                        Name = t.BuildingName,
                        SelectedRecipeIndex = t.SelectedRecipeIndex,
                        ReadyCount = t.Queue.Count(x => x.State == EnumWorkshopState.ReadyToPickUpManually)
                    };
                    output.Add(summary);

                });
                return output;
            }
        }
    }
    public int GetCapcity(WorkshopView summary)
    {
        WorkshopInstance workshop = GetWorkshopById(summary);
        return workshop.Capacity;
    }
    public CraftingSummary? GetSingleCraftedItem(WorkshopView summary, int index)
    {
        lock (_lock)
        {
            WorkshopInstance workshop = GetWorkshopById(summary);
            //was one based.
            int reals = index - 1;
            if (reals < 0)
            {
                return null;
            }
            try
            {
                CraftingJobInstance job = workshop.Queue[reals];
                CraftingSummary craftSummary = new()
                {
                    Id = job.Id,
                    Name = job.Recipe.Item,
                    State = job.State,
                    ReadyTime = job.State == EnumWorkshopState.Waiting
                            ? "Waiting"
                            : job.ReadyTime?.GetTimeCompact!
                };
                return craftSummary;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
    public void StartCraftingJob(WorkshopView summary, string item)
    {
        lock (_lock)
        {
            if (CanCraft(summary, item) == false)
            {
                throw new CustomBasicException("Unable to craft.  Should had ran the CanCraft first");
            }
            WorkshopRecipe recipe = _recipes.Single(x => x.Item == item);

            inventory.Consume(recipe.Inputs);
            CraftingJobInstance job = new(recipe, _multiplier);
            WorkshopInstance workshop = GetWorkshopById(summary);
            workshop.Queue.Add(job);
        }
    }
    public WorkshopView? SearchForWorkshop(string searchFor)
    {
        // Find the recipe that produces the desired item
        WorkshopRecipe? target = _recipes.FirstOrDefault(x => x.Item == searchFor); //you may have more than one.   if more than one, has to choose the first one.  you are on your own from here.
        if (target is null)
        {
            return null;
        }

        // Find the workshop instance that owns that recipe
        WorkshopInstance t = _workshops.First(x => x.BuildingName == target.BuildingName); //has to be first now because you can have more than one workshop with the same name.

        // IMPORTANT: compute the index inside that workshop's recipe list
        // Use the SAME ordering concept your UI relies on.
        var workshopRecipes = _recipes
            .Where(r => r.BuildingName == t.BuildingName)
            .ToList();

        int index = workshopRecipes.FindIndex(r => r.Item == searchFor);
        if (index >= 0)
        {
            // Persist selection so when UI loads it already matches
            t.SelectedRecipeIndex = index;
        }



        return new WorkshopView
        {
            Id = t.Id,
            Name = t.BuildingName,
            SelectedRecipeIndex = index,
            ReadyCount = t.Queue.Count(x => x.State == EnumWorkshopState.ReadyToPickUpManually)
        };
    }
    public void UpdateSelectedRecipe(WorkshopView id, int selectedIndex)
    {
        var workshop = GetWorkshopById(id);
        workshop.SelectedRecipeIndex = selectedIndex;
        //OnWorkshopsUpdated?.Invoke();
        _needsSaving = true;
    }
    public BasicList<WorkshopAvailabilityState> GetAllWorkshops
    {
        get
        {
            lock (_lock)
            {
                BasicList<WorkshopAvailabilityState> output = [];
                _workshops.ForEach(t =>
                {
                    bool remaining = false;
                    if (t.Queue.Count != 0)
                    {
                        remaining = true;
                    }
                    WorkshopAvailabilityState summary = new()
                    {
                        Id = t.Id,
                        Name = t.BuildingName,
                        Unlocked = t.Unlocked,
                        RemainingCraftingJobs = remaining
                    };
                    output.Add(summary);
                });
                return output;
            }

        }
    }
    public async Task<bool> CanUnlockWorkshopAsync(string name)
    {
        if (_workshopProgressionPolicy is null)
        {
            return false;
        }
        var list = GetAllWorkshops;
        var policy = await _workshopProgressionPolicy.CanUnlockAsync(list, name);
        return policy;
    }
    public async Task UnlockWorkshopAsync(string name)
    {
        var list = GetAllWorkshops;
        var policy = await _workshopProgressionPolicy!.UnlockAsync(list, name);
        UpdateWorkshopInstance(policy);
    }
    public async Task<bool> CanLockWorkshopAsync(string name)
    {
        if (_workshopProgressionPolicy is null)
        {
            return false;
        }
        var list = GetAllWorkshops;
        var policy = await _workshopProgressionPolicy!.CanLockAsync(list, name);
        return policy;
    }
    public async Task LockWorkshopAsync(string name)
    {

        var list = GetAllWorkshops;
        var policy = await _workshopProgressionPolicy!.LockAsync(list, name);
        UpdateWorkshopInstance(policy);
    }
    private void UpdateWorkshopInstance(WorkshopAvailabilityState summary)
    {
        var workshop = GetWorkshopById(summary.Id);
        workshop.Unlocked = summary.Unlocked;
        NotifyWorkshopsUpdated();
        _needsSaving = true;
    }
    private void NotifyWorkshopsUpdated()
    {
        OnWorkshopsUpdated?.Invoke();
    }

    public BasicList<WorkshopRecipeSummary> GetRecipesForWorkshop(WorkshopView summary)
    {
        // _multiplier should be the CURRENT workshop time multiplier (<= 1.0)
        double m = _multiplier;

        return _recipes
            .Where(r => r.BuildingName == summary.Name)
            .Select(r => new WorkshopRecipeSummary
            {
                Item = r.Item,
                Inputs = r.Inputs,
                Output = r.Output,

                // Show effective duration in the UI
                Duration = r.Duration.Apply(m),

                // Optional: expose base too if you want later
                // BaseDuration = r.Duration
            })
            .ToBasicList();
    }
    public bool AnyInQueue(WorkshopView summary)
    {
        lock (_lock)
        {
            WorkshopInstance workshop = GetWorkshopById(summary);
            return workshop.Queue.Count != 0;
        }
    }
    public BasicList<CraftingSummary> GetItemsBeingCrafted(WorkshopView summary)
    {
        lock (_lock)
        {
            WorkshopInstance workshop = GetWorkshopById(summary);
            BasicList<CraftingSummary> output = [];
            foreach (var job in workshop.Queue)
            {
                string readyTime = job.State switch
                {
                    EnumWorkshopState.ReadyToPickUpManually => "Ready",
                    EnumWorkshopState.Waiting => "Waiting",
                    EnumWorkshopState.Active => job.ReadyTime?.GetTimeString ?? "",
                    _ => ""
                };

                output.Add(new CraftingSummary
                {
                    Id = job.Id,
                    Name = job.Recipe.Item,
                    State = job.State,
                    ReadyTime = readyTime
                });
            }
            return output;
        }
    }
    public bool CanCraft(WorkshopView summary, string item)
    {
        lock (_lock)
        {
            WorkshopRecipe recipe = _recipes.Single(x => x.Item == item);
            if (recipe.BuildingName != summary.Name)
            {
                return false;
            }
            WorkshopInstance workshop = GetWorkshopById(summary);
            if (workshop.CanAccept(recipe) == false)
            {
                return false;
            }
            return inventory.Has(recipe.Inputs);
        }
    }
    public async Task StartCraftingJobAsync(WorkshopView summary, string item)
    {
        _automateCollection = await _workshopCollectionPolicy!.IsAutomaticAsync(); //has to do this before the lock.
        lock (_lock)
        {
            if (CanCraft(summary, item) == false)
            {
                throw new CustomBasicException("Unable to craft.  Should had ran the CanCraft first");
            }
            WorkshopRecipe recipe = _recipes.Single(x => x.Item == item);
            inventory.Consume(recipe.Inputs);
            CraftingJobInstance job = new(recipe, _multiplier);
            WorkshopInstance workshop = GetWorkshopById(summary);
            workshop.Queue.Add(job);
            _needsSaving = true;
        }
    }
    //public int GetCompletedCount(WorkshopView workshop)
    //{
    //    var instance = GetWorkshopById(workshop); // or however you map view->instance
    //    return instance.Queue.Count(x => x.State == EnumWorkshopState.ReadyToPickUpManually);
    //}
    public bool CanPickupManually(WorkshopView summary)
    {
        lock (_lock)
        {
            WorkshopInstance workshop = GetWorkshopById(summary);
            return workshop.Queue.Any(x => x.State == EnumWorkshopState.ReadyToPickUpManually);
        }
    }
    public void PickupManually(WorkshopView summary)
    {
        lock (_lock)
        {
            WorkshopInstance workshop = GetWorkshopById(summary);
            CraftingJobInstance active = workshop.Queue.First(x => x.State == EnumWorkshopState.ReadyToPickUpManually);
            //active.Complete();

            workshop.Queue.RemoveSpecificItem(active);
            //save something here too.
            inventory.Add(active.Recipe.Output.Item, active.Recipe.Output.Amount);
            _needsSaving = true;
            NotifyWorkshopsUpdated();
            //return workshop.Queue.Any(x => x.State == EnumWorkshopState.ReadyToPickUpManually);
            //CraftingJob? nexts = workshop.Queue.FirstOrDefault(x => x.State == EnumWorkshopState.ReadyToPickUpManually);
            //return nexts is not null;
        }
    }
    public async Task SetStyleContextAsync(WorkshopServicesContext context, FarmKey farm)
    {
        if (_workshopPersistence != null)
        {
            throw new InvalidOperationException("Persistance Already set");
        }
        BaseBalanceProfile profile = await baseBalanceProvider.GetBaseBalanceAsync(farm);
        _multiplier = profile.WorkshopTimeMultiplier;
        _workshopPersistence = context.WorkshopPersistence;
        _workshopProgressionPolicy = context.WorkshopProgressionPolicy;
        _workshopCollectionPolicy = context.WorkshopCollectionPolicy;
        _automateCollection = await _workshopCollectionPolicy.IsAutomaticAsync();
        _recipes = await context.WorkshopRegistry.GetWorkshopRecipesAsync();
        foreach (var item in _recipes)
        {
            itemRegistry.Register(new(item.Output.Item, EnumInventoryStorageCategory.Barn, EnumInventoryItemCategory.Workshops));
        }
        var ours = await context.WorkshopInstances.GetWorkshopInstancesAsync();
        _workshops.Clear();
        foreach (var item in ours)
        {
            WorkshopInstance workshop = new()
            {
                BuildingName = item.Name
            };
            workshop.Load(item, _recipes, _multiplier);
            _workshops.Add(workshop);
        }
    }
    public async Task UpdateTickAsync()
    {
        _workshops.ForConditionalItems(x => x.Unlocked, ProcessBuilding);
        await SaveWorkshopsAsync();
    }
    public async Task<bool> CanAutomateCollectionAsync()
    {
        _automateCollection = await _workshopCollectionPolicy!.IsAutomaticAsync();
        return _automateCollection;
    }

    private void ProcessBuilding(WorkshopInstance workshop)
    {
        if (workshop.Unlocked == false)
        {
            return;
        }

        // Find active job or start one
        var active = workshop.Queue.FirstOrDefault(j => j.State == EnumWorkshopState.Active);
        if (active == null)
        {
            var next = workshop.Queue.FirstOrDefault(j => j.State == EnumWorkshopState.Waiting);
            if (next != null)
            {
                next.Start();
                _needsSaving = true;
            }
            active = next;
        }

        if (active == null)
        {
            return;
        }

        var now = DateTime.Now;
        var elapsed = now - active.StartedAt!.Value;

        while (active != null && elapsed >= active.DurationForProcessing)
        {
            // Consume time for this job
            elapsed -= active.DurationForProcessing;

            if (_automateCollection)
            {
                inventory.Add(active.Recipe.Output.Item, active.Recipe.Output.Amount);
                active.Complete();
                workshop.Queue.RemoveSpecificItem(active);
                _needsSaving = true;
            }
            else
            {
                active.ReadyForManualPickup();
                NotifyWorkshopsUpdated();
                _needsSaving = true;
                return; // stop processing until player picks up
            }

            // Start next job immediately
            var next = workshop.Queue.FirstOrDefault(j => j.State == EnumWorkshopState.Waiting);
            if (next == null)
            {
                return;
            }

            next.Start();
            active = next;
            active.UpdateStartedAt(now - elapsed);
            _needsSaving = true;
        }
    }
    private async Task SaveWorkshopsAsync()
    {
        bool doSave = false;

        lock (_lock)
        {
            if (_needsSaving && DateTime.Now - _lastSave > GameRegistry.SaveThrottle)
            {
                _needsSaving = false;
                _lastSave = DateTime.Now;
                doSave = true;
            }
        }

        if (doSave == false)
        {
            return;
        }
        BasicList<WorkshopAutoResumeModel> models;
        lock (_lock)
        {
            models = _workshops
                .Select(w => w.GetWorkshopForSaving)
                .ToBasicList();
        }

        await _workshopPersistence.SaveWorkshopsAsync(models);
    }


}