namespace Phase11StoreWithBasicPurchases.ImportClasses;
internal static class ImportWorkerProgressClass
{
    public static async Task ImportWorkersAsync()
    {
        var firsts = FarmHelperClass.GetAllFarms();
        BasicList<WorkerProgressionPlanDocument> list = [];
        foreach (var item in firsts)
        {
            list.Add(GeneratePlanFarm(item));
        }
        WorkerProgressionPlanDatabase db = new();
        await db.ImportAsync(list);
    }
    private static WorkerProgressionPlanDocument GeneratePlanFarm(FarmKey farm)
    {
        WorkerProgressionPlanDocument document = new()
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
            ItemName = TropicalWorkerListClass.George,
            LevelRequired = 5
        });
        output.Add(new()
        {
            ItemName = TropicalWorkerListClass.Ethan,
            LevelRequired = 12
        });
        output.Add(new()
        {
            ItemName = TropicalWorkerListClass.Fiona,
            LevelRequired = 18
        });
        return output;
    }
    private static BasicList<ItemUnlockRule> GetUnlockRulesForCountry()
    {
        BasicList<ItemUnlockRule> output = [];
        output.Add(new()
        {
            ItemName = CountryWorkerListClass.Bob,
            LevelRequired = 7
        });
        output.Add(new()
        {
            ItemName = CountryWorkerListClass.Alice,
            LevelRequired = 9
        });
        output.Add(new()
        {
            ItemName = CountryWorkerListClass.Clara,
            LevelRequired = 12
        });
        output.Add(new()
        {
            ItemName = CountryWorkerListClass.Daniel,
            LevelRequired = 16
        });


        output.Add(new()
        {
            ItemName = CountryWorkerListClass.Emma,
            LevelRequired = 18
        });
        return output;
    }
}