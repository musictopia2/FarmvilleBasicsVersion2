namespace Phase11StoreWithBasicPurchases.ImportClasses;
internal static class ImportWorkshopCatalogClass
{
    private static EnumCatalogCategory _category = EnumCatalogCategory.Workshop;
    public static BasicList<CatalogOfferModel> GetWorkshopOffers(FarmKey farm)
    {
        if (farm.Theme == FarmThemeList.Tropical)
        {
            return GetCatalogForTropical();
        }
        if (farm.Theme == FarmThemeList.Country)
        {
            return GetCatalogForCountry();
        }
        throw new CustomBasicException("Not Supported");
    }
    private static BasicList<CatalogOfferModel> GetCatalogForTropical()
    {
        BasicList<CatalogOfferModel> output = [];
        output.Add(new()
        {
            TargetName = TropicalWorkshopList.HuluHit,
            LevelRequired = 2,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = TropicalWorkshopList.HuluHit,
            LevelRequired = 5,
            Category = _category,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(50) //this is where i do the balancing.
        });
        output.Add(new()
        {
            TargetName = TropicalWorkshopList.SushiStand,
            LevelRequired = 4,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = TropicalWorkshopList.SushiStand,
            LevelRequired = 8,
            Category = _category,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(60)
        });
        output.Add(new()
        {
            TargetName = TropicalWorkshopList.Grill,
            LevelRequired = 5,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });

        output.Add(new()
        {
            TargetName = TropicalWorkshopList.Grill,
            LevelRequired = 9,
            Category = _category,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(80)
        });

        output.Add(new()
        {
            TargetName = TropicalWorkshopList.BeachfrontKitchen,
            LevelRequired = 11,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = TropicalWorkshopList.BeachfrontKitchen,
            LevelRequired = 13,
            Category = _category,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(90) //this is where i do the balancing.
        });
        return output;
    }
    private static BasicList<CatalogOfferModel> GetCatalogForCountry()
    {
        BasicList<CatalogOfferModel> output = [];
        output.Add(new()
        {
            TargetName = CountryWorkshopList.Windmill,
            LevelRequired = 2,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = CountryWorkshopList.Windmill,
            LevelRequired = 6,
            Category = _category,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(10)
        });


        output.Add(new()
        {
            TargetName = CountryWorkshopList.PastryOven,
            LevelRequired = 4,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = CountryWorkshopList.PastryOven,
            LevelRequired = 8,
            Category = _category,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(20)
        });

        output.Add(new()
        {
            TargetName = CountryWorkshopList.StovetopOven,
            LevelRequired = 11,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = CountryWorkshopList.StovetopOven,
            LevelRequired = 14,
            Category = _category,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(30)
        });
        output.Add(new()
        {
            TargetName = CountryWorkshopList.Loom,
            LevelRequired = 15,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = CountryWorkshopList.Loom,
            LevelRequired = 16,
            Category = _category,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(40)
        });
        return output;
    }
}