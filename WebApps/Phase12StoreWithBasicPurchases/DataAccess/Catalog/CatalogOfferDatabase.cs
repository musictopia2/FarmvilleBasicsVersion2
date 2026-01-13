namespace Phase12StoreWithBasicPurchases.DataAccess.Catalog;
public class CatalogOfferDatabase() : ListDataAccess<CatalogOfferDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, ICatalogDataSource
{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "CatalogOffer"; 
    async Task<BasicList<CatalogOfferModel>> ICatalogDataSource.GetCatalogAsync(FarmKey farm)
    {
        var list = await GetDocumentsAsync();
        return list.GetSingleDocument(farm).Offers;
    }
}