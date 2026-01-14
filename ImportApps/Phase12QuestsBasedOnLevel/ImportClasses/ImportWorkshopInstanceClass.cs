namespace Phase12QuestsBasedOnLevel.ImportClasses;
internal static class ImportWorkshopInstanceClass
{
    private static WorkshopProgressionPlanDatabase _workshopProgression = null!;
    private static CatalogOfferDatabase _catalogdb = null!;
    private static ProgressionProfileDatabase _levelProfile = null!;
    private static BasicList<WorkshopRecipeDocument> _recipes = null!; //still needs this because when you have a building, you need to know what is associated with it.
    public static async Task ImportWorkshopsAsync()
    {
        _catalogdb = new();
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
        var catalogList = await _catalogdb.GetCatalogAsync(farm, EnumCatalogCategory.Workshop);
        WorkshopCapacityUpgradePlanDatabase capacityDb = new();
        BasicList<WorkshopCapacityUpgradePlanDocument> upgrades = await capacityDb.GetUpgradesAsync(farm);
        foreach (var catalog in catalogList)
        {
            WorkshopCapacityUpgradePlanDocument currentPlan = upgrades.Single(x => x.WorkshopName == catalog.TargetName);
            //needs this for some things.
            bool unlocked;
            if (catalog.LevelRequired > level || catalog.Costs.Count > 0)
            {
                unlocked = false;
            }
            else
            {
                unlocked = true;
            }
            WorkshopAutoResumeModel workshop = new()
            {
                Name = catalog.TargetName,
                Capacity = GetStartingWorkshopCapacity(currentPlan),
                Unlocked = unlocked
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