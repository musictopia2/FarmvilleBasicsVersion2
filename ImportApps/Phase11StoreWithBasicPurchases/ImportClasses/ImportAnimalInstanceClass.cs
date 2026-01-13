namespace Phase11StoreWithBasicPurchases.ImportClasses;
public static class ImportAnimalInstanceClass
{
    private static AnimalProgressionPlanDatabase _animalProgression = null!;
    private static ProgressionProfileDatabase _levelProfile = null!;
    private static BasicList<AnimalRecipeDocument> _recipes = [];
    public static async Task ImportAnimalsAsync()
    {

        _animalProgression = new();
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
        

        // Future:
        // list.Add(CreateFarm(PlayerList.Andy, FarmThemeList.Country, ProfileIdList.Test, animalsPerRecipe: 2));

        AnimalInstanceDatabase db = new();
        await db.ImportAsync(list);
    }

    //this only allows 1 now (until i rethink).

    private static async Task<AnimalInstanceDocument> CreateInstanceAsync(FarmKey farm)
    {
        BasicList<AnimalAutoResumeModel> animals = [];

        var animalPlan = await _animalProgression.GetPlanAsync(farm);
        var profile = await _levelProfile.GetProfileAsync(farm);
        int level = profile.Level;

        // 1) Unique animal list comes from the PLAN (not recipes)
        var allAnimalNames = animalPlan.UnlockRules
            .Select(x => x.ItemName)
            .Distinct()
            .ToBasicList();

        // 2) For fast counting, group all rules by animal name
        var rulesByAnimal = animalPlan.UnlockRules
            .GroupBy(x => x.ItemName)
            .ToDictionary(g => g.Key, g => g.ToBasicList());
        //OutputReady = recipe.Options.First().Output.Amount
        foreach (var animalName in allAnimalNames)
        {
            var rules = rulesByAnimal[animalName];

            // duplicates = more options; eligible rules = earned options
            int earned = rules.Count(r => r.LevelRequired <= level);

            bool unlocked = earned > 0;

            // If you don't want recipes involved here, keep it as "earned" and clamp later.
            int productionOptionsAllowed = unlocked ? earned : 1; //has to be at least one anyways (even though can't use yet).
            EnumAnimalState state = EnumAnimalState.None;
            int ready = 0;
            if (unlocked)
            {
                state = EnumAnimalState.Collecting;
                var recipe = _recipes.Single(x => x.Animal ==  animalName);
                ready = recipe.Options.First().Output.Amount;
            }
            animals.Add(new AnimalAutoResumeModel
            {
                Name = animalName,
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