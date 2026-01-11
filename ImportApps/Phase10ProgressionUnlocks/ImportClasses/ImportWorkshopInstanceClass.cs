namespace Phase10ProgressionUnlocks.ImportClasses;
internal static class ImportWorkshopInstanceClass
{
    private static BasicList<WorkshopRecipeDocument> _recipes = [];
    // MVP1 rule: each farm has 1 instance of each workshop building available in its Theme/Profile.
    // Future profiles/modes can allow more.
    private const int _productionWorkshopsPerBuilding = 2; //needs to be 2 so i can test upgrading 2 different building capacities

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
        list.Add(await CreateProductionAsync(PlayerList.Player1, FarmThemeList.Country));
        list.Add(await CreateProductionAsync(PlayerList.Player2, FarmThemeList.Country));
        list.Add(await CreateProductionAsync(PlayerList.Player1, FarmThemeList.Tropical));
        list.Add(await CreateProductionAsync(PlayerList.Player2, FarmThemeList.Tropical));

        // Future:
        // list.Add(CreateFarm(PlayerList.Andy, FarmThemeList.Country, ProfileIdList.Test, workshopsPerBuilding: 2));

        WorkshopInstanceDatabase db = new();
        await db.ImportAsync(list);
    }

    private static Task<WorkshopInstanceDocument> CreateProductionAsync(string playerName, string theme)
        => CreateFarmAsync(playerName, theme, ProfileIdList.Test, _productionWorkshopsPerBuilding);

    private static Task<WorkshopInstanceDocument> CreateFarmAsync(string playerName, string theme, string profileId, int workshopsPerBuilding)
    {
        var farm = new FarmKey(playerName, theme, profileId);
        return CreateInstanceAsync(farm, workshopsPerBuilding);
    }

    private static int GetStartingWorkshopCapacity(
        WorkshopCapacityUpgradePlanDocument plan,
        int freeTierCount = 2)
    {
        if (plan.Upgrades == null || plan.Upgrades.Count == 0)
        {
            throw new CustomBasicException($"No upgrade tiers for workshop '{plan.WorkshopName}'.");
        }

        // last free tier index (0-based list)
        int index = freeTierCount - 1;

        if (index < 0)
        {
            // if you ever set freeTierCount=0, start at first tier
            index = 0;
        }

        if (index >= plan.Upgrades.Count)
        {
            throw new CustomBasicException(
                $"Workshop '{plan.WorkshopName}' has only {plan.Upgrades.Count} tiers; " +
                $"cannot use freeTierCount={freeTierCount}.");
        }

        int capacity = plan.Upgrades[index].Size;

        if (capacity <= 0)
        {
            throw new CustomBasicException(
                $"Workshop '{plan.WorkshopName}' invalid starting capacity={capacity} from tier[{index}].");
        }

        return capacity;
    }

    private static async Task<WorkshopInstanceDocument> CreateInstanceAsync(FarmKey farm, int workshopsPerBuilding)
    {
        BasicList<WorkshopAutoResumeModel> workshops = [];

        WorkshopCapacityUpgradePlanDatabase capacityDb = new();
        BasicList<WorkshopCapacityUpgradePlanDocument> upgrades = await capacityDb.GetUpgradesAsync(farm);
        if (upgrades.Count == 0)
        {
            throw new CustomBasicException("No upgrades");
        }
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
        //if i want to show you already received some, set the values instead.
        //my version can do something different later.
        foreach (var building in buildings)
        {
            workshopsPerBuilding.Times(_ =>
            {
                WorkshopCapacityUpgradePlanDocument currentPlan = upgrades.Single(x => x.WorkshopName == building);
                workshops.Add(new WorkshopAutoResumeModel
                {
                    Name = building,
                    Capacity = GetStartingWorkshopCapacity(currentPlan)
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