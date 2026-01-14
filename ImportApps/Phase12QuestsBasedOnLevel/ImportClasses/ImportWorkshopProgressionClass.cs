namespace Phase12QuestsBasedOnLevel.ImportClasses;
public static class ImportWorkshopProgressionClass
{
    public static async Task ImportWorkshopsAsync()
    {
        var firsts = FarmHelperClass.GetAllFarms();
        BasicList<WorkshopProgressionPlanDocument> list = [];
        foreach (var item in firsts)
        {
            list.Add(GeneratePlanFarm(item));
        }
        WorkshopProgressionPlanDatabase db = new();
        await db.ImportAsync(list);
    }
    private static WorkshopProgressionPlanDocument GeneratePlanFarm(FarmKey farm)
    {
        WorkshopProgressionPlanDocument document = new()
        {
            Farm = farm
        };
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
        //Validate(document);
        return document;
    }
    private static BasicList<ItemUnlockRule> GetUnlockRulesForTropical()
    {
        BasicList<ItemUnlockRule> output = [];
        output.Add(new()
        {
            ItemName = TropicalItemList.PineappleSmoothie,
            LevelRequired = 2
        });
        output.Add(new()
        {
            ItemName = TropicalItemList.PinaColada,
            LevelRequired = 3
        });
        output.Add(new()
        {
            ItemName = TropicalItemList.SteamedRice,
            LevelRequired = 4
        });
        output.Add(new()
        {
            ItemName = TropicalItemList.FishRoll,
            LevelRequired = 5
        });
        output.Add(new()
        {
            ItemName = TropicalItemList.GrilledCrab,
            LevelRequired = 5
        });
        output.Add(new()
        {
            ItemName = TropicalItemList.FriedRice,
            LevelRequired = 6
        });
        output.Add(new()
        {
            ItemName = TropicalItemList.Ceviche,
            LevelRequired = 11
        });
        output.Add(new()
        {
            ItemName = TropicalItemList.TruffleFries,
            LevelRequired = 12
        });
        return output;
    }
    private static BasicList<ItemUnlockRule> GetUnlockRulesForCountry()
    {
        BasicList<ItemUnlockRule> output = [];
        output.Add(new()
        {
            ItemName = CountryItemList.Flour,
            LevelRequired = 2
        });
        output.Add(new()
        {
            ItemName = CountryItemList.Sugar,
            LevelRequired = 3
        });
        output.Add(new() //can change for reals anyways
        {
            ItemName = CountryItemList.Biscuit,
            LevelRequired = 4
        });
        output.Add(new()
        {
            ItemName = CountryItemList.ApplePie,
            LevelRequired = 5
        });
        output.Add(new()
        {
            ItemName = CountryItemList.FarmersSoup,
            LevelRequired = 11
        });
        output.Add(new()
        {
            ItemName = CountryItemList.GranolaBar,
            LevelRequired = 15
        });
        output.Add(new()
        {
            ItemName = CountryItemList.Trousers,
            LevelRequired = 15
        });
        output.Add(new()
        {
            ItemName = CountryItemList.Blanket,
            LevelRequired = 16
        });
        return output;
    }
}