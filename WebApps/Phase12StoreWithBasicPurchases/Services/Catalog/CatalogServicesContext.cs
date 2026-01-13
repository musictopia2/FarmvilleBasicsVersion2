namespace Phase12StoreWithBasicPurchases.Services.Catalog;
public class CatalogServicesContext
{
    public required ICatalogDataSource CatalogDataSource { get; init; }
}