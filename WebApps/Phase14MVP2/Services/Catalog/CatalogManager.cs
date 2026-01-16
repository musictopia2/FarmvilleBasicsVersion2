namespace Phase14MVP2.Services.Catalog;
public class CatalogManager
{
    private BasicList<CatalogOfferModel> _offers = [];
    public async Task SetCatalogStyleContextAsync(CatalogServicesContext context,
        FarmKey farm)
    {
        _offers = await context.CatalogDataSource.GetCatalogAsync(farm);
        if (_offers.Count == 0)
        {
            throw new CustomBasicException("No Offers");
        }
    }
    public BasicList<CatalogOfferModel> GetAllOffers(EnumCatalogCategory category) =>
    _offers.Where(x => x.Category == category)
           
           .Select(x => x.DeepCopy())
           .ToBasicList();

    public BasicList<CatalogOfferModel> GetFreeOffers(EnumCatalogCategory category) =>
        _offers.Where(x => x.Category == category && x.Costs.Count == 0)
               .OrderBy(x => x.TargetName)
               .ThenBy(x => x.LevelRequired)
               .Select(x => x.DeepCopy())
               .ToBasicList();

    //public BasicList<CatalogOfferModel> GetTargetOffers(EnumCatalogCategory category, string targetName) =>
    //    _offers.Where(x => x.Category == category && x.TargetName == targetName)
    //           .OrderBy(x => x.LevelRequired)
    //           .ThenBy(x => x.Costs.Count == 0 ? 0 : 1)
    //           .ThenBy(GetCostKey)
    //           .Select(x => x.DeepCopy())
    //           .ToBasicList();
    //private static string GetCostKey(CatalogOfferModel offer)
    //{
    //    if (offer.Costs == null || offer.Costs.Count == 0)
    //    {
    //        return ""; // free always sorts first anyway
    //    }

    //    // Stable representation: order keys so dictionary enumeration order doesn't matter
    //    return string.Join("|",
    //        offer.Costs
    //            .OrderBy(kv => kv.Key, StringComparer.Ordinal)
    //            .Select(kv => $"{kv.Key}:{kv.Value}"));
    //}

}