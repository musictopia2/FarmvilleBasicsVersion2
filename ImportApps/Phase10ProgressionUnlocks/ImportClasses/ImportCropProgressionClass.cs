namespace Phase10ProgressionUnlocks.ImportClasses;
public static class ImportCropProgressionClass
{
    public static async Task ImportCropsAsync()
    {
        var firsts = FarmHelperClass.GetAllFarms();
        BasicList<CropProgressionPlanDocument> list = [];
        foreach (var item in firsts)
        {
            list.Add(GeneratePlanFarm(item));
        }
        CropProgressionPlanDatabase db = new();
        await db.ImportAsync(list);
    }
    private static void AddSlots(BasicList<int> slots, int count, int levelRequired)
    {
        count.Times(() => slots.Add(levelRequired));
    }
    private static CropProgressionPlanDocument GeneratePlanFarm(FarmKey farm)
    {
        CropProgressionPlanDocument document = new()
        {
            Farm = farm
        };

        //here is where i specify the slot levels.
        document.SlotLevelRequired = [];
        AddSlots(document.SlotLevelRequired, 8, 1);
        AddSlots(document.SlotLevelRequired, 4, 3);
        AddSlots(document.SlotLevelRequired, 4, 6);
        AddSlots(document.SlotLevelRequired, 4, 11);
        if (farm.Theme == FarmThemeList.Tropical)
        {
            document.UnlockRules = GetUnlockRulesForTropical();
        }
        else if (farm.Theme == FarmThemeList.Country)
        {
            document.UnlockRules = GetUnlockRulesForCountry();
        }
        else
        {
            throw new CustomBasicException("Not supported");
        } 
        Validate(document);
        return document;

    }

    private static BasicList<ItemUnlockRule> GetUnlockRulesForTropical()
    {
        BasicList<ItemUnlockRule> output = [];
        output.Add(new()
        {
            LevelRequired = 1,
            ItemName = TropicalItemList.Pineapple
        });
        output.Add(new()
        {
            LevelRequired = 2,
            ItemName = TropicalItemList.Rice
        });
        output.Add(new()
        {
            LevelRequired = 9,
            ItemName = TropicalItemList.Tapioca
        });

        return output;
    }

    private static void Validate(CropProgressionPlanDocument doc)
    {
        if (doc.SlotLevelRequired.Count == 0)
        {
            throw new CustomBasicException("SlotLevelRequired cannot be empty");
        }

        if (doc.SlotLevelRequired.Any(x => x < 1))
        {
            throw new CustomBasicException("All slot unlock levels must be >= 1");
        }

        if (doc.UnlockRules is null || doc.UnlockRules.Count == 0)
        {
            throw new CustomBasicException("UnlockRules cannot be empty");
        }

        if (doc.UnlockRules.Any(x => x.LevelRequired < 1))
        {
            throw new CustomBasicException("All item unlock levels must be >= 1");
        }

        var dupes = doc.UnlockRules
            .GroupBy(x => x.ItemName)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToBasicList();

        if (dupes.Count > 0)
        {
            throw new CustomBasicException($"Duplicate item unlock rules: {string.Join(", ", dupes)}");
        }
    }

    private static BasicList<ItemUnlockRule> GetUnlockRulesForCountry()
    {
        BasicList<ItemUnlockRule> output = [];
        output.Add(new()
        {
            LevelRequired = 1,
            ItemName = CountryItemList.Wheat
        });
        output.Add(new()
        {
            LevelRequired = 2,
            ItemName = CountryItemList.Corn
        });
        output.Add(new()
        {
            LevelRequired = 4,
            ItemName = CountryItemList.Carrot
        });
        output.Add(new()
        {
            LevelRequired = 8,
            ItemName = CountryItemList.Strawberry
        });
        output.Add(new()
        {
            LevelRequired = 9,
            ItemName = CountryItemList.Tomato
        });
        output.Add(new()
        {
            LevelRequired = 14,
            ItemName = CountryItemList.HoneyComb
        });
        return output;
    }
}