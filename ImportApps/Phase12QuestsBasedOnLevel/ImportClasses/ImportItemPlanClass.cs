namespace Phase12QuestsBasedOnLevel.ImportClasses;
public static class ImportItemPlanClass
{
    public static async Task ImportItemsAsync()
    {
        var farms = FarmHelperClass.GetAllFarms();
        BasicList<ItemPlanDocument> list = [];
        foreach (var farm in farms)
        {
            list.Add(await GetPlanAsync(farm));
        }
        ItemPlanDatabase db = new();
        await db.ImportAsync(list);
    }
    private static async Task<ItemPlanDocument> GetPlanAsync(FarmKey farm)
    {
        BasicList<ItemPlanModel> items = await GetPossibleItemsAsync(farm);
        return new()
        {
            Farm = farm,
            ItemList = items
        };
    }
    private sealed record ItemPlanInfo(int MinLevel, EnumItemCategory Category, string Source);

    private static void AddOrLower(
        Dictionary<string, ItemPlanInfo> dict,
        string item,
        int level,
        EnumItemCategory category,
        string source)
    {
        if (string.IsNullOrWhiteSpace(item))
        {
            return;
        }

        source ??= "";

        var next = new ItemPlanInfo(level, category, source);

        if (dict.TryGetValue(item, out var existing))
        {
            // Keep earliest level. If equal, prefer non-empty source.
            if (level < existing.MinLevel)
            {
                dict[item] = next;
            }
            else if (level == existing.MinLevel &&
                     string.IsNullOrWhiteSpace(existing.Source) &&
                     !string.IsNullOrWhiteSpace(source))
            {
                dict[item] = next;
            }
        }
        else
        {
            dict[item] = next;
        }
    }

    private static void AddIfMissing(
        Dictionary<string, ItemPlanInfo> dict,
        string item,
        int level,
        EnumItemCategory category,
        string source)
    {
        if (string.IsNullOrWhiteSpace(item))
        {
            return;
        }

        if (!dict.ContainsKey(item))
        {
            dict[item] = new ItemPlanInfo(level, category, source ?? "");
        }
    }

    private static async Task<BasicList<ItemPlanModel>> GetPossibleItemsAsync(FarmKey farm)
    {
        Dictionary<string, ItemPlanInfo> itemMap = new(StringComparer.OrdinalIgnoreCase);

        // -----------------------------
        // 1) CROPS (direct unlocks)
        // -----------------------------
        var cropPlanDb = new CropProgressionPlanDatabase();
        var cropPlan = await cropPlanDb.GetPlanAsync(farm);

        foreach (var rule in cropPlan.UnlockRules)
        {
            // Source blank for crops (or set to rule.ItemName if you want)
            AddOrLower(itemMap, rule.ItemName, rule.LevelRequired, EnumItemCategory.Crop, "");
        }

        // -----------------------------
        // 2) CATALOG-UNLOCKED BUILDINGS (trees, worksites, animals)
        // -----------------------------
        var catalogDb = new CatalogOfferDatabase();

        async Task<Dictionary<string, int>> GetMinUnlockByTargetAsync(EnumCatalogCategory cat)
        {
            var offers = await catalogDb.GetCatalogAsync(farm, cat);
            return offers
                .GroupBy(x => x.TargetName, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.Min(x => x.LevelRequired), StringComparer.OrdinalIgnoreCase);
        }

        var treeUnlock = await GetMinUnlockByTargetAsync(EnumCatalogCategory.Tree);
        var worksiteUnlock = await GetMinUnlockByTargetAsync(EnumCatalogCategory.Worksite);
        var animalUnlock = await GetMinUnlockByTargetAsync(EnumCatalogCategory.Animal);

        // -----------------------------
        // 3) TREES -> FRUIT ITEMS
        // -----------------------------
        var treeRecipesDb = new TreeRecipeDatabase();
        var treeRecipes = await treeRecipesDb.GetRecipesAsync();

        foreach (var tr in treeRecipes.Where(x => x.Theme == farm.Theme))
        {
            
            string treeName = tr.TreeName;
            string fruitItem = tr.Item;

            if (treeUnlock.TryGetValue(treeName, out int lvl))
            {
                AddOrLower(itemMap, fruitItem, lvl, EnumItemCategory.Tree, treeName);
            }
        }

        // -----------------------------
        // 4) WORKSITES -> ITEMS (baseline benefits; skip Optional=true)
        // -----------------------------
        var worksiteRecipesDb = new WorksiteRecipeDatabase();
        var worksiteRecipes = await worksiteRecipesDb.GetRecipesAsync();

        foreach (var ws in worksiteRecipes.Where(x => x.Theme == farm.Theme))
        {
            string location = ws.Location;

            if (!worksiteUnlock.TryGetValue(location, out int wsLevel))
            {
                continue;
            }

            foreach (var benefit in ws.BaselineBenefits)
            {
                if (benefit.Optional)
                {
                    continue;
                }

                AddOrLower(itemMap, benefit.Item, wsLevel, EnumItemCategory.Worksite, location);
            }
        }

        // -----------------------------
        // 5) ANIMALS -> OUTPUT ITEMS
        // Rules:
        //   - Option #1 unlocks when animal unlocks in catalog.
        //   - Option #2+ unlock levels come from AnimalProgressionPlan.UnlockRules.
        //   - For option #2+, only add if output item is NOT already in map (unique).
        // -----------------------------
        var animalPlanDb = new AnimalProgressionPlanDatabase();
        var animalPlan = await animalPlanDb.GetPlanAsync(farm);

        // animal -> sorted levels for additional options only
        var extraOptionLevelsByAnimal = animalPlan.UnlockRules
            .GroupBy(x => x.ItemName, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.LevelRequired).OrderBy(x => x).ToList(),
                StringComparer.OrdinalIgnoreCase);

        var animalRecipesDb = new AnimalRecipeDatabase();
        var animalRecipes = await animalRecipesDb.GetRecipesAsync();

        foreach (var ar in animalRecipes.Where(x => x.Theme == farm.Theme))
        {
            string animalName = ar.Animal;

            if (!animalUnlock.TryGetValue(animalName, out int animalBaseLevel))
            {
                continue;
            }

            extraOptionLevelsByAnimal.TryGetValue(animalName, out var extraLevels);
            extraLevels ??= [];

            var options = ar.Options;
            if (options.Count == 0)
            {
                continue;
            }

            for (int optionIndex = 0; optionIndex < options.Count; optionIndex++)
            {
                int optionUnlockLevel;

                if (optionIndex == 0)
                {
                    optionUnlockLevel = animalBaseLevel;
                }
                else
                {
                    int extraIdx = optionIndex - 1;
                    if (extraIdx >= extraLevels.Count)
                    {
                        continue; // no progression rule for that extra option
                    }
                    optionUnlockLevel = extraLevels[extraIdx];
                }

                var opt = options[optionIndex];

                // required input must already be known (usually crop)
                if (!itemMap.TryGetValue(opt.Required, out var reqInfo))
                {
                    continue;
                }

                int finalLevel = Math.Max(optionUnlockLevel, reqInfo.MinLevel);
                string outputItem = opt.Output.Item;

                // Unique rule (you asked for this): if already exists, ignore.
                if (itemMap.ContainsKey(outputItem))
                {
                    continue;
                }

                AddIfMissing(itemMap, outputItem, finalLevel, EnumItemCategory.Animal, animalName);
            }
        }

        // -----------------------------
        // 6) WORKSHOPS -> CRAFTED OUTPUT ITEMS
        // MinLevel is from WorkshopProgressionPlan.UnlockRules.
        // Source comes from workshop recipe (building name).
        // -----------------------------
        var workshopPlanDb = new WorkshopProgressionPlanDatabase();
        var workshopPlan = await workshopPlanDb.GetPlanAsync(farm);

        var recipeUnlockByOutput = workshopPlan.UnlockRules
            .GroupBy(x => x.ItemName, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.Min(x => x.LevelRequired), StringComparer.OrdinalIgnoreCase);

        var workshopRecipesDb = new WorkshopRecipeDatabase();
        var workshopRecipes = await workshopRecipesDb.GetRecipesAsync();

        // output item -> building/workshop name (source)
        var outputToBuilding = workshopRecipes
            .Where(x => x.Theme == farm.Theme)
            .GroupBy(x => x.Output.Item, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First().BuildingName, StringComparer.OrdinalIgnoreCase);

        foreach (var kvp in recipeUnlockByOutput)
        {
            string outputItem = kvp.Key;
            int unlockLevel = kvp.Value;

            outputToBuilding.TryGetValue(outputItem, out string? buildingName);
            buildingName ??= "";

            AddOrLower(itemMap, outputItem, unlockLevel, EnumItemCategory.Workshop, buildingName);
        }

        ValidateWorkshopInputs(
            farm,
            workshopRecipes.Where(x => x.Theme == farm.Theme),
            itemMap,
            recipeUnlockByOutput);

        // -----------------------------
        // 7) Convert to ItemPlanModel list
        // -----------------------------
        BasicList<ItemPlanModel> output = [];

        foreach (var kvp in itemMap.OrderBy(x => x.Value.MinLevel).ThenBy(x => x.Key))
        {
            output.Add(new ItemPlanModel
            {
                MinLevel = kvp.Value.MinLevel,
                ItemName = kvp.Key,
                Category = kvp.Value.Category,
                Source = kvp.Value.Source
            });
        }

        return output;
    }

    private static void ValidateWorkshopInputs(
        FarmKey farm,
        IEnumerable<WorkshopRecipeDocument> recipes,
        Dictionary<string, ItemPlanInfo> itemMap,
        Dictionary<string, int> recipeUnlockByOutput)
    {
        foreach (var r in recipes)
        {
            string outputItem = r.Output.Item;

            if (!recipeUnlockByOutput.TryGetValue(outputItem, out int unlockLevel))
            {
                continue;
            }

            foreach (var input in r.Inputs.Keys)
            {
                if (!itemMap.TryGetValue(input, out var info))
                {
                    throw new CustomBasicException(
                        $"ItemPlan invalid for {farm.Theme}: output '{outputItem}' unlocks at L{unlockLevel} but input '{input}' has no known unlock level.");
                }

                if (info.MinLevel > unlockLevel)
                {
                    throw new CustomBasicException(
                        $"ItemPlan invalid for {farm.Theme}: output '{outputItem}' unlocks at L{unlockLevel} but input '{input}' unlocks at L{info.MinLevel}.");
                }
            }
        }
    }


}