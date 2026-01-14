namespace Phase13QuestsBasedOnLevel.DataAccess.Catalog;
public class CatalogOfferDocument : IFarmDocument
{
    public required FarmKey Farm { get; init; }
    public required BasicList<CatalogOfferModel> Offers { get; init; } = [];
}