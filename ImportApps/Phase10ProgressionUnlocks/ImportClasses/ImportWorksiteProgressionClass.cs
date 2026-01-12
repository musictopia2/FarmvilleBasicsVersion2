namespace Phase10ProgressionUnlocks.ImportClasses;
public class ImportWorksiteProgressionClass
{
    public static async Task ImportWorksitesAsync()
    {
        var firsts = FarmHelperClass.GetAllFarms();
        BasicList<WorksiteProgressionPlanDocument> list = [];
        foreach (var item in firsts)
        {
            list.Add(GeneratePlanFarm(item));
        }
        WorksiteProgressionPlanDatabase db = new();
        await db.ImportAsync(list);
    }
    private static  WorksiteProgressionPlanDocument GeneratePlanFarm(FarmKey farm)
    {
        WorksiteProgressionPlanDocument document = new()
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
            ItemName = TropicalWorksiteListClass.CorelReef,
            LevelRequired = 5
        });
        output.Add(new()
        {
            ItemName = TropicalWorksiteListClass.HotSprings,
            LevelRequired = 12
        });
        output.Add(new()
        {
            ItemName = TropicalWorksiteListClass.SmugglersCave,
            LevelRequired = 18
        });
        return output;
    }
    private static BasicList<ItemUnlockRule> GetUnlockRulesForCountry()
    {
        BasicList<ItemUnlockRule> output = [];
        output.Add(new()
        {
            ItemName = CountryWorksiteListClass.GrandmasGlade,
            LevelRequired = 7
        });
        output.Add(new()
        {
            ItemName = CountryWorksiteListClass.Pond,
            LevelRequired = 12
        });
        output.Add(new()
        {
            ItemName = CountryWorksiteListClass.Mines,
            LevelRequired = 18
        });
        return output;
    }
}