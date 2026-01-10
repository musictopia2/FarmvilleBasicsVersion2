namespace Phase08UpgradeWorkshopCapacity.ImportClasses;
public static class ImportWorkshopCapacityUpgradesClass
{
    public static async Task ImportWorkshopsAsync()
    {
        var farms = FarmHelperClass.GetAllFarms();
        WorkshopRecipeDatabase others = new();
        var recipes = await others.GetRecipesAsync();

        BasicList<WorkshopCapacityUpgradePlanDocument> list = [];
        foreach (var farm in farms)
        {
            // Unique building names for this theme
            var buildingNames = recipes
                .Where(r => r.Theme == farm.Theme)
                .Select(r => r.BuildingName)
                .Where(name => string.IsNullOrWhiteSpace(name) == false)
                .Distinct()
                .ToBasicList();

            foreach (var buildingName in buildingNames)
            {
                var upgrades = GetUpgrades(buildingName);
                ValidateWorkshopCapacityTiers(upgrades, buildingName, farm);

                list.Add(new WorkshopCapacityUpgradePlanDocument
                {
                    Farm = farm,
                    WorkshopName = buildingName,
                    Upgrades = upgrades
                });
            }
        }
        WorkshopCapacityUpgradePlanDatabase db = new();
        await db.ImportAsync(list);
    }

    private static BasicList<UpgradeTier> GetUpgrades(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new CustomBasicException("Must provide a name.   Used for future");
        }
        //this will show the first 2 are free.
        //only allow up to 9.
        //later will figure out the costs.
        BasicList<UpgradeTier> output = [];

        2.Times(x =>
        {
            UpgradeTier tier = new()
            {
                Cost = FarmHelperClass.GetFreeCosts,
                Size = x
            };
            output.Add(tier);
        });

        int currentCost = 2;
        int capacity = 3;

        UpgradeTier tier = new()
        {
            Cost = FarmHelperClass.GetCoinOnlyDictionary(currentCost),
            Size = capacity
        };
        output.Add(tier);
        capacity++; //4

        currentCost++;

        tier = new()
        {
            Cost = FarmHelperClass.GetCoinOnlyDictionary(currentCost),
            Size = capacity
        };
        output.Add(tier);

        capacity++; //5

        currentCost+=2;

        tier = new()
        {
            Cost = FarmHelperClass.GetCoinOnlyDictionary(currentCost),
            Size = capacity
        };
        output.Add(tier);

        capacity++; //6

        currentCost += 5;

        tier = new()
        {
            Cost = FarmHelperClass.GetCoinOnlyDictionary(currentCost),
            Size = capacity
        };
        output.Add(tier);

        capacity++; //7

        currentCost += 4;

        tier = new()
        {
            Cost = FarmHelperClass.GetCoinOnlyDictionary(currentCost),
            Size = capacity
        };
        output.Add(tier);

        capacity++; //8

        currentCost += 4;

        tier = new()
        {
            Cost = FarmHelperClass.GetCoinOnlyDictionary(currentCost),
            Size = capacity
        };
        output.Add(tier);
        capacity++; //9

        currentCost += 5;

        tier = new()
        {
            Cost = FarmHelperClass.GetCoinOnlyDictionary(currentCost),
            Size = capacity
        };
        output.Add(tier);
        return output;
    }


    private static void ValidateWorkshopCapacityTiers(
        BasicList<UpgradeTier> tiers,
        string workshopName,
        FarmKey farm,
        int freeTierCount = 2,
        int maxCapacity = 9)
    {
        if (string.IsNullOrWhiteSpace(workshopName))
        {
            throw new CustomBasicException($"Workshop name blank for {farm}");
        }

        if (tiers == null || tiers.Count == 0)
        {
            throw new CustomBasicException($"Workshop capacity upgrades empty for {workshopName} ({farm})");
        }
        for (int i = 0; i < tiers.Count; i++)
        {
            var t = tiers[i];

            if (t.Size <= 0)
            {
                throw new CustomBasicException($"{workshopName} tier[{i}] invalid Size={t.Size} for {farm}");
            }

            if (t.Size != i + 1)
            {
                throw new CustomBasicException($"{workshopName} tier[{i}] must have a size of {i + 1}, not {t.Size}");
            }

            if (t.Size > maxCapacity)
            {
                throw new CustomBasicException(
                    $"{workshopName} tier[{i}] Size={t.Size} exceeds max {maxCapacity} for {farm}");
            }

            bool shouldBeFree = i < freeTierCount;

            if (shouldBeFree)
            {
                if (t.Cost.Count > 0)
                {
                    throw new CustomBasicException($"{workshopName} tier[{i}] must be free for {farm}");
                }
            }
            else
            {
                if (t.Cost.Count == 0)
                {
                    throw new CustomBasicException($"{workshopName} tier[{i}] must have a cost for {farm}");
                }

                foreach (var c in t.Cost)
                {
                    if (string.IsNullOrWhiteSpace(c.Key))
                    {
                        throw new CustomBasicException($"{workshopName} tier[{i}] has blank cost item for {farm}");
                    }
                    if (c.Value <= 0)
                    {
                        throw new CustomBasicException(
                            $"{workshopName} tier[{i}] cost {c.Key} must be > 0 for {farm}");
                    }
                }
            }
        }
    }

}