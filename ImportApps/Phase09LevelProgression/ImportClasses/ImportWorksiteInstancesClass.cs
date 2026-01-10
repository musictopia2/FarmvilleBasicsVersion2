namespace Phase09LevelProgression.ImportClasses;
public static class ImportWorksiteInstancesClass
{
    private static BasicList<WorksiteRecipeDocument> _recipes = [];

    public static async Task ImportWorksitesAsync()
    {
        WorksiteRecipeDatabase recipeDb = new();
        _recipes = await recipeDb.GetRecipesAsync();

        if (_recipes.Count == 0)
        {
            throw new CustomBasicException("No worksite recipes were imported.");
        }

        BasicList<WorksiteInstanceDocument> list = [];

        // MVP1 Production farms
        list.Add(CreateProduction(PlayerList.Player1, FarmThemeList.Country));
        list.Add(CreateProduction(PlayerList.Player2, FarmThemeList.Country));
        list.Add(CreateProduction(PlayerList.Player1, FarmThemeList.Tropical));
        list.Add(CreateProduction(PlayerList.Player2, FarmThemeList.Tropical));

        // Future:
        // list.Add(CreateFarm(PlayerList.Andy, FarmThemeList.Country, ProfileIdList.Test));

        WorksiteInstanceDatabase db = new();
        await db.ImportAsync(list);
    }

    private static WorksiteInstanceDocument CreateProduction(string playerName, string theme)
        => CreateFarm(playerName, theme, ProfileIdList.Test);

    private static WorksiteInstanceDocument CreateFarm(string playerName, string theme, string profileId)
    {
        var farm = new FarmKey(playerName, theme, profileId);
        return CreateInstance(farm);
    }

    private static WorksiteInstanceDocument CreateInstance(FarmKey farm)
    {
        BasicList<WorksiteAutoResumeModel> worksites = [];

        // One instance per distinct location for this Theme/Profile
        var locations = _recipes
            .Where(r => r.Theme == farm.Theme)
            .Select(r => r.Location)
            .Distinct()
            .ToBasicList();

        if (locations.Count == 0)
        {
            throw new CustomBasicException(
                $"No worksite locations found for Theme='{farm.Theme}' ProfileId='{farm.ProfileId}'.");
        }

        foreach (var location in locations)
        {
            worksites.Add(new WorksiteAutoResumeModel
            {
                Name = location
            });
        }

        return new WorksiteInstanceDocument
        {
            Farm = farm,
            Worksites = worksites
        };
    }
}