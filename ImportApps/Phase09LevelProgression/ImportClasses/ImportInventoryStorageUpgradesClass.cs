namespace Phase09LevelProgression.ImportClasses;
internal static class ImportInventoryStorageUpgradesClass
{
    public static async Task ImportInventoryStorageUpgradesAsync()
    {
        var firsts = FarmHelperClass.GetAllFarms();
        BasicList<InventoryStorageUpgradePlanDocument> list = [];
        foreach (var farm in firsts)
        {
            BasicList<UpgradeTier> barns = GetBarnUpgrades();
            BasicList<UpgradeTier> silos = GetSiloUpgrades();
            ValidateTiers(barns, "Barn", farm);
            ValidateTiers(silos, "Silo", farm);
            InventoryStorageUpgradePlanDocument upgrade = new()
            {
                Farm = farm,
                BarnUpgrades = barns,
                SiloUpgrades = silos,
            };
            list.Add(upgrade);
        }
        InventoryStorageUpgradePlanDatabase db = new();
        await db.ImportAsync(list);
    }
    private static BasicList<UpgradeTier> GetBarnUpgrades()
    {
        UpgradeTier tier;
        BasicList<UpgradeTier> output = [];
        int currentValue = 50;
        int increment = 4;
        tier = new()
        {
            Cost = FarmHelperClass.GetFreeCosts,
            Size = currentValue
        };
        output.Add(tier);
        currentValue += increment;
        int currentCost = 2;
        tier = new()
        {
            Cost = FarmHelperClass.GetCoinOnlyDictionary(currentCost),
            Size = currentValue
        };
        output.Add(tier);
        currentCost += 1;
        increment += 1;
        currentValue += increment;
        tier = new()
        {
            Cost = FarmHelperClass.GetCoinOnlyDictionary(currentCost),
            Size = currentValue
        };
        output.Add(tier);
        currentCost += 2;
        currentValue += increment;
        tier = new()
        {
            Cost = FarmHelperClass.GetCoinOnlyDictionary(currentCost),
            Size = currentValue
        };
        output.Add(tier);
        return output;

    }
    private static BasicList<UpgradeTier> GetSiloUpgrades()
    {
        UpgradeTier tier;
        BasicList<UpgradeTier> output = [];
        int currentValue = 100;
        int increment = 5;
        tier = new()
        {
            Cost = FarmHelperClass.GetFreeCosts,
            Size = currentValue
        };
        output.Add(tier);
        currentValue += increment;
        int currentCost = 5;
        tier = new()
        {
            Cost = FarmHelperClass.GetCoinOnlyDictionary(currentCost),
            Size = currentValue
        };
        output.Add(tier);
        currentCost += 3;
        increment += 1;
        currentValue += increment;
        tier = new()
        {
            Cost = FarmHelperClass.GetCoinOnlyDictionary(currentCost),
            Size = currentValue
        };
        output.Add(tier);
        currentCost += 2;
        currentValue += increment;
        tier = new()
        {
            Cost = FarmHelperClass.GetCoinOnlyDictionary(currentCost),
            Size = currentValue
        };
        output.Add(tier);
        return output;
    }

    
    

    private static void ValidateTiers(
        BasicList<UpgradeTier> tiers,
        string name,
        FarmKey farm)
    {
        if (tiers.Count == 0)
        {
            throw new CustomBasicException($"{name} upgrades empty for {farm}");
        }

        int lastSize = -1;
        for (int i = 0; i < tiers.Count; i++)
        {
            var t = tiers[i];
            if (t.Size <= 0)
            {
                throw new CustomBasicException($"{name} tier[{i}] has invalid Size={t.Size} for {farm}");
            }
            if (t.Size <= lastSize)
            {
                throw new CustomBasicException(
                    $"{name} tier[{i}] Size={t.Size} must be > {lastSize} for {farm}");
            }

            // Tier 0 must be free
            if (i == 0)
            {
                if (t.Cost.Count > 0)
                {
                    throw new CustomBasicException($"{name} tier[0] must be free for {farm}");
                }
            }
            else
            {
                // Tier 1+ must have a cost
                if (t.Cost.Count == 0)
                {
                    throw new CustomBasicException($"{name} tier[{i}] must have a cost for {farm}");
                }
            }
            foreach (var c in t.Cost)
            {
                if (string.IsNullOrWhiteSpace(c.Key))
                {
                    throw new CustomBasicException($"{name} tier[{i}] has blank cost item for {farm}");
                }

                if (c.Value <= 0)
                {
                    throw new CustomBasicException($"{name} tier[{i}] cost {c.Key} must be > 0 for {farm}");
                }
            }
            lastSize = t.Size;
        }
    }
}