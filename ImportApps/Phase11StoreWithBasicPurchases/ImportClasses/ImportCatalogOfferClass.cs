namespace Phase11StoreWithBasicPurchases.ImportClasses;
public static class ImportCatalogOfferClass
{
    public static async Task ImportCatalogAsync()
    {
        CatalogOfferDatabase db = new();
        var firsts = FarmHelperClass.GetAllFarms();
        BasicList<CatalogOfferDocument> list = [];
        foreach (var item in firsts)
        {
            list.Add(GenerateCatalogFarm(item));
        }
        await db.ImportAsync(list);
    }
    private static CatalogOfferDocument GenerateCatalogFarm(FarmKey farm)
    {

        BasicList<CatalogOfferModel> list = [];
        list.AddRange(ImportTreeCatalogClass.GetTreeOffers(farm));
        list.AddRange(ImportAnimalCatalogClass.GetAnimalOffers(farm));
        //do for other domains.

        return new()
        {
            Farm = farm,
            Offers = list

        };

    }
}

