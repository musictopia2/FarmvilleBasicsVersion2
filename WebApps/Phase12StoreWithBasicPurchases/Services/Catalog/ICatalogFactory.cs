namespace Phase12StoreWithBasicPurchases.Services.Catalog;
public interface ICatalogFactory
{
    CatalogServicesContext GetCatalogServices(FarmKey farm);
}