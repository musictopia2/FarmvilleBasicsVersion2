namespace Phase12QuestsBasedOnLevel.ImportClasses;
public static class ImportTreeCatalogClass
{
    private static EnumCatalogCategory _category = EnumCatalogCategory.Tree;
    public static BasicList<CatalogOfferModel> GetTreeOffers(FarmKey farm)
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
            TargetName = TropicalTreeListClass.Coconut,
            LevelRequired = 1,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = TropicalTreeListClass.Coconut,
            LevelRequired = 3,
            Category = _category,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(5) //this is where i do the balancing.
        });
        output.Add(new()
        {
            TargetName = TropicalTreeListClass.Lime,
            LevelRequired = 8,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = TropicalTreeListClass.Lime,
            LevelRequired = 9,
            Category = _category,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(10) //this is where i do the balancing.
        });
        return output;
    }
    private static BasicList<CatalogOfferModel> GetCatalogForCountry()
    {
        BasicList<CatalogOfferModel> output = [];
        output.Add(new()
        {
            TargetName = CountryTreeListClass.Apple, //no option for second apple tree.
            LevelRequired = 1,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = CountryTreeListClass.Peach,
            LevelRequired = 9,
            Category = _category,
            Costs = FarmHelperClass.GetFreeCosts
        });
        output.Add(new()
        {
            TargetName = CountryTreeListClass.Peach,
            LevelRequired = 10,
            Category = _category,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(10)
        });
        output.Add(new()
        {
            TargetName = CountryTreeListClass.Peach,
            LevelRequired = 14,
            Category = _category,
            Costs = FarmHelperClass.GetCoinOnlyDictionary(20)
        });
        return output;
    }

}