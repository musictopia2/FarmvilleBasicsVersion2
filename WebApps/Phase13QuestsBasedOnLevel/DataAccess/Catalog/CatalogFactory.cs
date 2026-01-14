namespace Phase13QuestsBasedOnLevel.DataAccess.Catalog;
public class CatalogFactory : ICatalogFactory
{
    CatalogServicesContext ICatalogFactory.GetCatalogServices(FarmKey farm)
    {
        return new()
        {
            CatalogDataSource = new CatalogOfferDatabase()
        };
    }
}