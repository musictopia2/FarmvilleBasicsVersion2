namespace Phase11StoreWithBasicPurchases.DataAccess;
public class CatalogOfferDatabase() : ListDataAccess<CatalogOfferDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "CatalogOffer";
    public async Task ImportAsync(BasicList<CatalogOfferDocument> list)
    {
        await UpsertRecordsAsync(list);
    }

    public async Task<BasicList<CatalogOfferModel>> GetCatalogAsync(FarmKey farm, EnumCatalogCategory category)
    {
        var list = await GetDocumentsAsync();
        return list.GetSingleDocument(farm).Offers.Where(x => x.Category == category).ToBasicList();
    }
}