namespace Phase14MVP2.DataAccess.Catalog;
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