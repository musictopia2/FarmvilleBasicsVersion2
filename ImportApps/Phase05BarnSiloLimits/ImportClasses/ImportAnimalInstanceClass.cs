namespace Phase05BarnSiloLimits.ImportClasses;
public static class ImportAnimalInstanceClass
{
    private static BasicList<AnimalRecipeDocument> _recipes = [];

    // MVP1 rule: each farm gets 1 of each animal type (per recipe) for its Theme/Profile.
    // Future profiles/modes can use different counts.
    private const int _productionAnimalsPerRecipe = 1;

    public static async Task ImportAnimalsAsync()
    {
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
            list.Add(CreateProduction(item.PlayerName, item.Theme));
        }
        

        // Future:
        // list.Add(CreateFarm(PlayerList.Andy, FarmThemeList.Country, ProfileIdList.Test, animalsPerRecipe: 2));

        AnimalInstanceDatabase db = new();
        await db.ImportAsync(list);
    }

    private static AnimalInstanceDocument CreateProduction(string playerName, string theme)
        => CreateFarm(playerName, theme, ProfileIdList.Test, _productionAnimalsPerRecipe);

    private static AnimalInstanceDocument CreateFarm(string playerName, string theme, string profileId, int animalsPerRecipe)
    {
        var farm = new FarmKey(playerName, theme, profileId);
        return CreateInstance(farm, animalsPerRecipe);
    }

    private static AnimalInstanceDocument CreateInstance(FarmKey farm, int animalsPerRecipe)
    {
        BasicList<AnimalAutoResumeModel> animals = [];

        // IMPORTANT: filter by the farm's Theme + ProfileId so you don't mix Test/Production
        var recipesForFarm = _recipes
            .Where(r => r.Theme == farm.Theme)
            .ToBasicList();

        if (recipesForFarm.Count == 0)
        {
            throw new CustomBasicException(
                $"No animal recipes found for Theme='{farm.Theme}' ProfileId='{farm.ProfileId}'.");
        }

        recipesForFarm.ForEach(recipe =>
        {
            animalsPerRecipe.Times(_ =>
            {
                animals.Add(new AnimalAutoResumeModel
                {
                    Name = recipe.Animal,
                    ProductionOptionsAllowed = recipe.Options.Count,
                    State = EnumAnimalState.Collecting,
                    Selected = 0,
                    OutputReady = recipe.Options.First().Output.Amount
                });
            });
        });

        return new AnimalInstanceDocument
        {
            Farm = farm,
            Animals = animals
        };
    }
}