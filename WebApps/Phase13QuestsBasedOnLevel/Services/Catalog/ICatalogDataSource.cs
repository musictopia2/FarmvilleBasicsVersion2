namespace Phase13QuestsBasedOnLevel.Services.Catalog;
public interface ICatalogDataSource
{
    Task<BasicList<CatalogOfferModel>> GetCatalogAsync(FarmKey farm);
}