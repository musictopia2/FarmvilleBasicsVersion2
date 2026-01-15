namespace Phase12QuestsBasedOnLevel.ImportClasses;
public static class ImportAnimalCatalogClass
{
    private static EnumCatalogCategory _category = EnumCatalogCategory.Animal;
    public static BasicList<CatalogOfferModel> GetAnimalOffers(FarmKey farm)
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
            TargetName = TropicalAnimalListClass.Dolphin,
            LevelRequired = 2,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = TropicalAnimalListClass.Dolphin,
            LevelRequired = 10,
            Category = _category,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(50) //this is where i do the balancing.
        });
        output.Add(new()
        {
            TargetName = TropicalAnimalListClass.Chicken,
            LevelRequired = 4,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = TropicalAnimalListClass.Chicken,
            LevelRequired = 10,
            Category = _category,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(60) //this is where i do the balancing.
        });
        output.Add(new()
        {
            TargetName = TropicalAnimalListClass.Boar,
            LevelRequired = 11,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = TropicalAnimalListClass.Boar,
            LevelRequired = 11,
            Category = _category,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(80) //this is where i do the balancing.
        });
        return output;
    }
    private static BasicList<CatalogOfferModel> GetCatalogForCountry()
    {
        BasicList<CatalogOfferModel> output = [];
        output.Add(new()
        {
            TargetName = CountryAnimalListClass.Cow,
            LevelRequired = 3,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = CountryAnimalListClass.Cow,
            LevelRequired = 5,
            Category = _category,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(20)
        });


        output.Add(new()
        {
            TargetName = CountryAnimalListClass.Goat,
            LevelRequired = 10,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = CountryAnimalListClass.Goat,
            LevelRequired = 14,
            Category = _category,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(30)
        });

        output.Add(new()
        {
            TargetName = CountryAnimalListClass.Sheep,
            LevelRequired = 14,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = CountryAnimalListClass.Sheep,
            LevelRequired = 14,
            Category = _category,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(40)
        });
        return output;
    }
}