namespace Phase12QuestsBasedOnLevel.ImportClasses;
public static class ImportAnimalInstanceClass
{
    private static AnimalProgressionPlanDatabase _animalProgression = null!;
    private static CatalogOfferDatabase _catalogOfferDatabase = null!;
    private static ProgressionProfileDatabase _levelProfile = null!;
    private static BasicList<AnimalRecipeDocument> _recipes = [];
    public static async Task ImportAnimalsAsync()
    {
        _animalProgression = new();
        _catalogOfferDatabase = new();
        _levelProfile = new();
        AnimalRecipeDatabase recipeDb = new();
        _recipes = await recipeDb.GetRecipesAsync();
        if (_recipes.Count == 0)
        {
            throw new CustomBasicException("No animal recipes were imported.");
        }
        BasicList<AnimalInstanceDocument> list = [];
        var firsts = FarmHelperClass.GetAllFarms();
        foreach (var item in firsts)
        {
            list.Add(await CreateInstanceAsync(item));
        }
        AnimalInstanceDatabase db = new();
        await db.ImportAsync(list);
    }
    private static async Task<AnimalInstanceDocument> CreateInstanceAsync(FarmKey farm)
    {
        BasicList<AnimalAutoResumeModel> animals = [];
        var animalPlan = await _animalProgression.GetPlanAsync(farm);
        var current = await _catalogOfferDatabase.GetCatalogAsync(farm, EnumCatalogCategory.Animal);
        var profile = await _levelProfile.GetProfileAsync(farm);
        int level = profile.Level;
        foreach (var item in current)
        {
            bool unlocked = level >= item.LevelRequired;
            if (item.Costs.Count > 0)
            {
                unlocked = false; //you have to pay for it first.
            }
            var productionOptionsAllowed = animalPlan.UnlockRules.Count(x => x.LevelRequired <= level && x.ItemName == item.TargetName);
            productionOptionsAllowed += 1;
            EnumAnimalState state = EnumAnimalState.None;
            int ready = 0;
            if (unlocked)
            {
                
                state = EnumAnimalState.Collecting;
                var recipe = _recipes.Single(x => x.Animal == item.TargetName);
                ready = recipe.Options.First().Output.Amount;
            }
            animals.Add(new AnimalAutoResumeModel
            {
                Name = item.TargetName,
                Unlocked = unlocked,
                State = state,
                ProductionOptionsAllowed = productionOptionsAllowed,
                OutputReady = ready
            });
        }
        return new AnimalInstanceDocument
        {
            Farm = farm,
            Animals = animals
        };
    }
}