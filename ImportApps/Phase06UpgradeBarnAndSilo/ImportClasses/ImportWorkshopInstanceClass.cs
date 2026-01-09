namespace Phase06UpgradeBarnAndSilo.ImportClasses;
internal static class ImportWorkshopInstanceClass
{
    private static BasicList<WorkshopRecipeDocument> _recipes = [];

    // MVP1 rule: each farm has 1 instance of each workshop building available in its Theme/Profile.
    // Future profiles/modes can allow more.
    private const int _productionWorkshopsPerBuilding = 1;

    public static async Task ImportWorkshopsAsync()
    {
        WorkshopRecipeDatabase recipeDb = new();
        _recipes = await recipeDb.GetRecipesAsync();

        if (_recipes.Count == 0)
        {
            throw new CustomBasicException("No workshop recipes were imported.");
        }

        BasicList<WorkshopInstanceDocument> list = [];

        // Production farms for MVP1
        list.Add(CreateProduction(PlayerList.Player1, FarmThemeList.Country));
        list.Add(CreateProduction(PlayerList.Player2, FarmThemeList.Country));
        list.Add(CreateProduction(PlayerList.Player1, FarmThemeList.Tropical));
        list.Add(CreateProduction(PlayerList.Player2, FarmThemeList.Tropical));

        // Future:
        // list.Add(CreateFarm(PlayerList.Andy, FarmThemeList.Country, ProfileIdList.Test, workshopsPerBuilding: 2));

        WorkshopInstanceDatabase db = new();
        await db.ImportAsync(list);
    }

    private static WorkshopInstanceDocument CreateProduction(string playerName, string theme)
        => CreateFarm(playerName, theme, ProfileIdList.Test, _productionWorkshopsPerBuilding);

    private static WorkshopInstanceDocument CreateFarm(string playerName, string theme, string profileId, int workshopsPerBuilding)
    {
        var farm = new FarmKey(playerName, theme, profileId);
        return CreateInstance(farm, workshopsPerBuilding);
    }

    private static WorkshopInstanceDocument CreateInstance(FarmKey farm, int workshopsPerBuilding)
    {
        BasicList<WorkshopAutoResumeModel> workshops = [];

        // Get the distinct buildings that exist for this farm’s Theme
        var buildings = _recipes
            .Where(r => r.Theme == farm.Theme )
            .Select(r => r.BuildingName)
            .Distinct()
            .ToBasicList();

        if (buildings.Count == 0)
        {
            throw new CustomBasicException(
                $"No workshop buildings found for Theme='{farm.Theme}' ProfileId='{farm.ProfileId}'.");
        }

        foreach (var building in buildings)
        {
            workshopsPerBuilding.Times(_ =>
            {
                workshops.Add(new WorkshopAutoResumeModel
                {
                    Name = building
                });
            });
        }

        return new WorkshopInstanceDocument
        {
            Farm = farm,
            Workshops = workshops
        };
    }
}