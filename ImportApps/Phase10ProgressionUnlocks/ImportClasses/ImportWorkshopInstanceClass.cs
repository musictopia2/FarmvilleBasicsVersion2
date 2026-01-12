namespace Phase10ProgressionUnlocks.ImportClasses;
internal static class ImportWorkshopInstanceClass
{
    private static WorkshopProgressionPlanDatabase _workshopProgression = null!;
    private static ProgressionProfileDatabase _levelProfile = null!;
    private static BasicList<WorkshopRecipeDocument> _recipes = null!;
    public static async Task ImportWorkshopsAsync()
    {
        WorkshopRecipeDatabase recipeDb = new(); //this time, i need recipes.
        _recipes = await recipeDb.GetRecipesAsync();
        if (_recipes.Count == 0)
        {
            throw new CustomBasicException("No workshop recipes were imported.");
        }
        _workshopProgression = new();
        _levelProfile = new();
        BasicList<WorkshopInstanceDocument> list = [];
        var farms = FarmHelperClass.GetAllFarms();
        foreach (var farm in farms)
        {
            list.Add(await CreateInstanceAsync(farm));
        }
        WorkshopInstanceDatabase db = new();
        await db.ImportAsync(list);
    }
    private static async Task<WorkshopInstanceDocument> CreateInstanceAsync(FarmKey farm)
    {
        BasicList<WorkshopAutoResumeModel> workshops = [];
        var workshopPlan = await _workshopProgression.GetPlanAsync(farm);
        var profile = await _levelProfile.GetProfileAsync(farm);
        int level = profile.Level;
        //the workshopplan knows nothing about buildngs.



        var buildings = _recipes
            .Where(r => r.Theme == farm.Theme)
            .Select(r => r.BuildingName)
            .Distinct()
            .ToBasicList();

        if (buildings.Count == 0)
        {
            throw new CustomBasicException(
                $"No workshop buildings found for Theme='{farm.Theme}' ProfileId='{farm.ProfileId}'.");
        }
        WorkshopCapacityUpgradePlanDatabase capacityDb = new();
        BasicList<WorkshopCapacityUpgradePlanDocument> upgrades = await capacityDb.GetUpgradesAsync(farm);
        foreach (var building in buildings)
        {
            WorkshopCapacityUpgradePlanDocument currentPlan = upgrades.Single(x => x.WorkshopName == building);
            //needs this for some things.

            WorkshopAutoResumeModel workshop = new()
            {
                Name = building,
                Capacity = GetStartingWorkshopCapacity(currentPlan)
            };

            workshops.Add(workshop);

        }

        foreach (var workshop in workshops)
        {
            var temps = _recipes.Where(x => x.BuildingName == workshop.Name).ToBasicList();
            foreach (var item in temps)
            {
                var rule = workshopPlan.UnlockRules.Single(x => x.ItemName == item.Item);
                bool unlocked = profile.Level >= rule.LevelRequired;
                UnlockModel fins = new()
                {
                    Name = item.Item,
                    Unlocked = unlocked
                };
                workshop.SupportedItems.Add(fins);
            }
        }
        return new()
        {
            Farm = farm,
            Workshops = workshops
        };
        
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

}