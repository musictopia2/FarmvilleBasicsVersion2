namespace Phase12QuestsBasedOnLevel.Models;
public class StoreUiStateDocument : IFarmDocument
{
    required public FarmKey Farm { get; set; }
    public EnumCatalogCategory LastCategory { get; set; } = EnumCatalogCategory.Tree;
}