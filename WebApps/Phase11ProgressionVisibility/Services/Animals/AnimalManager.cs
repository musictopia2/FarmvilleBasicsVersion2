namespace Phase11ProgressionVisibility.Services.Animals;
public class AnimalManager(InventoryManager inventory,
    IBaseBalanceProvider baseBalanceProvider,
    ItemRegistry itemRegistry
    )
{
    private readonly BasicList<AnimalInstance> _animals = [];
    public event Action? OnAnimalsUpdated;
    private IAnimalRepository _animalRepository = null!;


    //private IAnimalCollectionPolicy? _animalCollectionPolicy;
    private EnumAnimalCollectionMode _animalCollectionMode;
    private bool _needsSaving;
    private DateTime _lastSave = DateTime.MinValue;
    private readonly Lock _lock = new();
    private BasicList<AnimalRecipe> _recipes = [];
    public BasicList<AnimalView> GetUnlockedAnimals
    {
        get
        {
            BasicList<AnimalView> output = [];
            _animals.ForConditionalItems(x => x.Unlocked, t =>
            {
                AnimalView summary = new()
                {
                    Id = t.Id,
                    Name = t.Name
                };
                output.Add(summary);
            });
            return output;
        }
    }
    public BasicList<AnimalState> GetAllAnimals
    {
        get
        {
            BasicList<AnimalState> output = [];
            _animals.ForEach(t =>
            {
                bool processing = t.State != EnumAnimalState.None;
                output.Add(new()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Unlocked = t.Unlocked,
                    InProgress = processing,
                    TotalPossibleOptions = t.TotalProductionOptions,
                    TotalAllowedOptions = t.ProductionOptionsAllowed
                });
            });
            return output;
        }
    }
    public AnimalProductionOption NextProductionOption(string animal)
    {
        var instance = _animals.First(x => x.Name == animal);
        return instance.NextProductionOption;

    }
    public void ApplyAnimalProgressionUnlocks(BasicList<ItemUnlockRule> rules, int level)
    {
        bool changed = false;

        lock (_lock)
        {
            // Count how many option-unlock tokens have been earned for each animal
            var earnedCounts = rules
                .Where(r => r.LevelRequired <= level)
                .GroupBy(r => r.ItemName)
                .ToDictionary(g => g.Key, g => g.Count());

            foreach (var kvp in earnedCounts)
            {
                string animalName = kvp.Key;
                int earnedOptions = kvp.Value; // 1..n based on duplicates in plan

                // Hard invariant: animal instances are all preloaded
                AnimalInstance animal = _animals.Single(a => a.Name == animalName);

                // Unlock animal as soon as it has at least 1 earned token
                if (earnedOptions > 0 && animal.Unlocked == false)
                {
                    animal.Unlocked = true;
                    changed = true;
                    animal.State = EnumAnimalState.Collecting;

                    var recipe = _recipes.Single(x => x.Animal == animalName);
                    animal.UpdateReady(recipe.Options.First().Output.Amount);

                }

                // Allowed production options = earned tokens (capped)
                int desiredAllowed = Math.Min(earnedOptions, animal.TotalProductionOptions);

                if (animal.ProductionOptionsAllowed < desiredAllowed)
                {
                    animal.ProductionOptionsAllowed = desiredAllowed;
                    changed = true;
                }
            }

            if (changed)
            {
                _needsSaving = true;
            }
        }

        if (changed)
        {
            OnAnimalsUpdated?.Invoke();
        }
    }

    private AnimalInstance GetAnimalById(Guid id)
    {
        var tree = _animals.SingleOrDefault(t => t.Id == id) ?? throw new CustomBasicException($"Animal with Id {id} not found.");
        return tree;
    }
    private AnimalInstance GetAnimalById(AnimalView id) => GetAnimalById(id.Id);
    public bool HasAnimal(string item)
    {
        bool rets = false;
        _recipes.ForEach(recipe =>
        {
            if (rets == true)
            {
                return;
            }
            if (recipe.Options.Any(x => x.Output.Item == item))
            {
                rets = true;
            }
        });
        return rets;
    }
    public BasicList<AnimalProductionOption> GetUnlockedProductionOptions(AnimalView animal)
    {
        AnimalInstance instance = GetAnimalById(animal);
        return instance.GetUnlockedProductionOptions().ToBasicList();
    }
    public string GetName(AnimalView animal)
    {
        AnimalInstance instance = GetAnimalById(animal);
        return instance.Name;
    }
    public int Required(AnimalView animal, int selected) => GetAnimalById(animal).RequiredCount(selected);
    public int Returned(AnimalView animal, int selected) => GetAnimalById(animal).Returned(selected);
    public bool CanProduce(AnimalView animal, int selected)
    {
        AnimalInstance instance = GetAnimalById(animal);
        if (instance.State != EnumAnimalState.None)
        {
            return false;
        }
        int required = instance.RequiredCount(selected);
        int count = inventory.Get(instance.RequiredName(selected));
        return count >= required;
    }
    public void Produce(AnimalView animal, int selected)
    {
        AnimalInstance instance = GetAnimalById(animal);
        if (CanProduce(animal, selected) == false)
        {
            throw new CustomBasicException("Cannot produce animal.  Should had used CanProduce function");
        }
        int required = instance.RequiredCount(selected);
        inventory.Consume(instance.RequiredName(selected), required);
        instance.Produce(selected);
        _needsSaving = true;
    }
    private int GetAmount(AnimalInstance instance)
    {
        int maxs;
        if (_animalCollectionMode == EnumAnimalCollectionMode.OneAtTime)
        {
            maxs = 1;
        }
        else
        {
            maxs = instance.OutputReady;
        }
        return maxs;
    }
    private bool CanCollect(AnimalInstance instance)
    {
        int maxs = GetAmount(instance);
        return inventory.CanAdd(instance.ReceivedName, maxs);
    }
    public bool CanCollect(AnimalView animal)
    {
        if (_animalCollectionMode == EnumAnimalCollectionMode.Automated)
        {
            throw new CustomBasicException("Should had been automated");
        }
        AnimalInstance instance = GetAnimalById(animal);
        return CanCollect(instance);
        
    }
    public void Collect(AnimalView animal)
    {
        //if there is a change in collection mode, requires rethinking.
        //cannot be here because has to have validation that is not async now.

        if (_animalCollectionMode == EnumAnimalCollectionMode.Automated)
        {
            throw new CustomBasicException("Should had been automated");
        }
        AnimalInstance instance = GetAnimalById(animal);
        int maxs = GetAmount(instance);
        Collect(instance, maxs);
    }
    private void Collect(AnimalInstance animal, int maxs)
    {
        string selectedName = animal.ReceivedName;
        maxs.Times(x =>
        {
            animal.Collect();
        });
        inventory.Add(selectedName, maxs);
        _needsSaving = true;
    }
    public EnumAnimalState GetState(AnimalView animal) => GetAnimalById(animal).State;
    public int Left(AnimalView animal) => GetAnimalById(animal).OutputReady;
    public string TimeLeftForResult(AnimalView animal)
    {
        AnimalInstance instance = GetAnimalById(animal);
        if (instance.ReadyTime is null)
        {
            return "";
        }
        return instance.ReadyTime!.Value.GetTimeString;
    }
    public string Duration(AnimalView animal, int selected)
    {
        AnimalInstance instance = GetAnimalById(animal);
        return instance.GetDuration(selected).GetTimeString;
    }
    public int InProgress(AnimalView animal) => GetAnimalById(animal).AmountInProgress;
    public async Task SetStyleContextAsync(AnimalServicesContext context, FarmKey farm)
    {
        if (_animalRepository != null)
        {
            throw new InvalidOperationException("Persistance Already set");
        }
        _animalRepository = context.AnimalRepository;
        _animalCollectionMode = await context.AnimalCollectionPolicy.GetCollectionModeAsync();
        _recipes = await context.AnimalRegistry.GetAnimalsAsync();
        foreach (var item in _recipes)
        {
            foreach (var temp in item.Options)
            {
                itemRegistry.Register(new(temp.Output.Item, EnumInventoryStorageCategory.Barn, EnumInventoryItemCategory.Animals));
            }
        }
        var ours = await context.AnimalRepository.LoadAsync();
        _animals.Clear();
        BaseBalanceProfile profile = await baseBalanceProvider.GetBaseBalanceAsync(farm);
        double offset = profile.AnimalTimeMultiplier;
        foreach (var item in ours)
        {
            AnimalRecipe recipe = _recipes.Single(x => x.Animal == item.Name);
            AnimalInstance animal = new(recipe, offset);

            animal.Load(item);
            _animals.Add(animal);
        }
    }
    //this can be called on demand.

    //

    //public async Task CheckCollectionModeAsync()
    //{
    //    _animalCollectionMode = await _animalCollectionPolicy!.GetCollectionModeAsync();
    //}
    public async Task UpdateTickAsync()
    {
        _animals.ForConditionalItems(x => x.Unlocked && x.State != EnumAnimalState.None, animal =>
        {
            animal.UpdateTick();
            if (animal.State == EnumAnimalState.Collecting && _animalCollectionMode == EnumAnimalCollectionMode.Automated)
            {
                if (CanCollect(animal))
                {
                    Collect(animal, animal.OutputReady); //if you cannot collect, then still can't do.
                }
            }
            if (animal.NeedsToSave)
            {
                _needsSaving = true;
            }
        });
        await SaveAnimalsAsync();
    }
    private async Task SaveAnimalsAsync()
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
            BasicList<AnimalAutoResumeModel> list = _animals
             .Select(animal => animal.GetAnimalForSaving)
             .ToBasicList();
            await _animalRepository.SaveAsync(list);
        }
    }
}