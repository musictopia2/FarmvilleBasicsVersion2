namespace Phase10ProgressionUnlocks.ImportClasses;

public static class ImportTreeProgressionClass
{
    public static async Task ImportTreesAsync()
    {
        var firsts = FarmHelperClass.GetAllFarms();
        BasicList<TreeProgressionPlanDocument> list = [];
        foreach (var item in firsts)
        {
            list.Add(GeneratePlanFarm(item));
        }
        TreeProgressionPlanDatabase db = new();
        await db.ImportAsync(list);
    }
    private static TreeProgressionPlanDocument GeneratePlanFarm(FarmKey farm)
    {
        TreeProgressionPlanDocument document = new()
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
            ItemName = TropicalTreeListClass.Coconut,
            LevelRequired = 1
        });
        output.Add(new()
        {
            ItemName = TropicalTreeListClass.Lime,
            LevelRequired = 8
        });
        
        return output;
    }
    private static BasicList<ItemUnlockRule> GetUnlockRulesForCountry()
    {
        BasicList<ItemUnlockRule> output = [];
        output.Add(new()
        {
            ItemName = CountryTreeListClass.Apple,
            LevelRequired = 1
        });
        output.Add(new()
        {
            ItemName = CountryTreeListClass.Peach,
            LevelRequired = 9
        });
        
        return output;
    }
}