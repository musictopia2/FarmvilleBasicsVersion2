namespace Phase12StoreWithBasicPurchases.Services.Catalog;
public interface ICatalogDataSource
{
    Task<BasicList<CatalogOfferModel>> GetCatalogAsync(FarmKey farm);
}