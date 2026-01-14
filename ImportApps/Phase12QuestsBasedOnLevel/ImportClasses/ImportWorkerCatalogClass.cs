namespace Phase12QuestsBasedOnLevel.ImportClasses;
internal static class ImportWorkerCatalogClass
{
    private static EnumCatalogCategory _category = EnumCatalogCategory.Worker;
    public static BasicList<CatalogOfferModel> GetWorkerOffers(FarmKey farm)
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
            TargetName = TropicalWorkerListClass.George,
            LevelRequired = 5,
            Costs = FarmHelperClass.GetFreeCosts,
            Category= _category
        });
        output.Add(new()
        {
            TargetName = TropicalWorkerListClass.Ethan,
            LevelRequired = 12,
            Costs = FarmHelperClass.GetFreeCosts,
            Category = _category
        });
        output.Add(new()
        {
            TargetName = TropicalWorkerListClass.Fiona,
            LevelRequired = 18,
            Costs = FarmHelperClass.GetFreeCosts,
            Category = _category
        });
        output.Add(new()
        {
            TargetName = TropicalWorkerListClass.Toby,
            LevelRequired = 8,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(10), //has to charge at least one or can't test the charging of a worker.
            Category = _category
        });
        return output;
    }
    private static BasicList<CatalogOfferModel> GetCatalogForCountry()
    {
        BasicList<CatalogOfferModel> output = [];
        output.Add(new()
        {
            TargetName = CountryWorkerListClass.Bob,
            LevelRequired = 7,
            Costs = FarmHelperClass.GetFreeCosts,
            Category = _category
        });
        output.Add(new()
        {
            TargetName = CountryWorkerListClass.Alice,
            LevelRequired = 9,
            Costs = FarmHelperClass.GetFreeCosts,
            Category = _category
        });
        output.Add(new()
        {
            TargetName = CountryWorkerListClass.Clara,
            LevelRequired = 12,
            Costs = FarmHelperClass.GetFreeCosts,
            Category = _category
        });
        output.Add(new()
        {
            TargetName = CountryWorkerListClass.Daniel,
            LevelRequired = 16,
            Costs = FarmHelperClass.GetFreeCosts,
            Category = _category
        });
        output.Add(new()
        {
            TargetName = CountryWorkerListClass.Emma,
            LevelRequired = 18,
            Costs = FarmHelperClass.GetFreeCosts,
            Category = _category
        });
        return output;
    }
}