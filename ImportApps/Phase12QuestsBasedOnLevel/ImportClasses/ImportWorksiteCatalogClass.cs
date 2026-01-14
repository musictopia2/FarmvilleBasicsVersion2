namespace Phase12QuestsBasedOnLevel.ImportClasses;
internal static class ImportWorksiteCatalogClass
{
    private static EnumCatalogCategory _category = EnumCatalogCategory.Worksite;
    public static BasicList<CatalogOfferModel> GetWorksiteOffers(FarmKey farm)
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
            TargetName = TropicalWorksiteListClass.CorelReef,
            LevelRequired = 5,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        
        output.Add(new()
        {
            TargetName = TropicalWorksiteListClass.HotSprings,
            LevelRequired = 12,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = TropicalWorksiteListClass.SmugglersCave,
            LevelRequired = 18,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        return output;
    }
    private static BasicList<CatalogOfferModel> GetCatalogForCountry()
    {
        BasicList<CatalogOfferModel> output = [];
        output.Add(new()
        {
            TargetName = CountryWorksiteListClass.GrandmasGlade,
            LevelRequired = 7,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });

        output.Add(new()
        {
            TargetName = CountryWorksiteListClass.Pond,
            LevelRequired = 12,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = CountryWorksiteListClass.Mines,
            LevelRequired = 18,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        return output;
    }
}
