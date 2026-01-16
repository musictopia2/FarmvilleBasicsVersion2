namespace Phase14MVP2.Services.Catalog;
public class CatalogServicesContext
{
    public required ICatalogDataSource CatalogDataSource { get; init; }
}