namespace Phase13QuestsBasedOnLevel.Services.Catalog;
public class CatalogServicesContext
{
    public required ICatalogDataSource CatalogDataSource { get; init; }
}