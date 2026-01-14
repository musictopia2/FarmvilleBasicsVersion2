using System.Net.Http.Headers;

namespace Phase13QuestsBasedOnLevel.Services.Animals;
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
    public void UnlockAnimalPaidFor(StoreItemRowModel store)
    {
        if (store.Category != EnumCatalogCategory.Animal)
        {
            throw new CustomBasicException("Only animals can be paid for");
        }
        var instance = _animals.First(x => x.Name == store.TargetName && x.Unlocked == false);
        instance.Unlocked = true;


        instance.State = EnumAnimalState.Collecting;

        var recipe = _recipes.Single(x => x.Animal == store.TargetName);
        instance.UpdateReady(recipe.Options.First().Output.Amount);


        _needsSaving = true;
    }
    public void ApplyAnimalProgressionUnlocksFromLevels(
    BasicList<ItemUnlockRule> rules,
    BasicList<CatalogOfferModel> offers,
    int level)
    {
        bool changed = false;

        lock (_lock)
        {
            // -----------------------------
            // 1) Type-level counts by animal
            // -----------------------------

            // First option unlocked comes from offers (per animal type).
            // We treat "has any offer <= level" as "base option unlocked".
            // IMPORTANT: This does NOT mean the animal itself is unlocked.
            var hasBaseOption = offers
                .Where(o => o.LevelRequired <= level)
                .GroupBy(o => o.TargetName)
                .ToDictionary(g => g.Key, g => true);

            // Extra options come from rules (duplicates = extra unlock tokens)
            var extraCounts = rules
                .Where(r => r.LevelRequired <= level)
                .GroupBy(r => r.ItemName)
                .ToDictionary(g => g.Key, g => g.Count());

            // Union of all animal names that appear in either system
            var animalNames = hasBaseOption.Keys
                .Union(extraCounts.Keys)
                .ToBasicList();

            foreach (var animalName in animalNames)
            {
                // All instances (could be 0, 1, or many)
                var instances = _animals.Where(a => a.Name == animalName).ToBasicList();
                if (instances.Count == 0)
                {
                    // Hard invariant violated: plan refers to animal not preloaded
                    throw new CustomBasicException($"Animal instances not preloaded for '{animalName}'.");
                }

                int baseAllowed = hasBaseOption.ContainsKey(animalName) ? 1 : 0;
                int extraEarned = extraCounts.TryGetValue(animalName, out int extra) ? extra : 0;

                // Determine cap (assumes all instances share same TotalProductionOptions)
                int cap = instances[0].TotalProductionOptions;

                int desiredAllowed = Math.Min(baseAllowed + extraEarned, cap);

                // Apply to ALL instances, even if locked (so UI can show options)
                foreach (var animal in instances)
                {
                    if (animal.ProductionOptionsAllowed != desiredAllowed)
                    {
                        animal.ProductionOptionsAllowed = desiredAllowed;
                        changed = true;
                    }
                }
            }

            // -----------------------------------------
            // 2) Optional: unlock ONE instance by offer
            // -----------------------------------------
            // If your offers also represent "free unlock at level",
            // you can unlock a single locked instance here.
            //
            // If some animals are purchase-gated, you MUST gate this
            // with your own condition (example: offer is free).
            //
            // If you *never* want this method to unlock animals anymore,
            // you can delete this entire section.


            var offer = offers.FirstOrDefault(x => x.LevelRequired == level);
            if (offer is not null)
            {
                string animalName = offer.TargetName;


                var instance = _animals.First(a => a.Name == animalName && a.Unlocked == false);
                

                instance.Unlocked = true;
                instance.State = EnumAnimalState.Collecting;

                var recipe = _recipes.Single(x => x.Animal == animalName);
                instance.UpdateReady(recipe.Options.First().Output.Amount);

                changed = true;
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