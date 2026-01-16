namespace Phase14MVP2.DataAccess.Store;
public class StoreUiStateDocument : IFarmDocument
{
    required public FarmKey Farm { get; set; }
    public EnumCatalogCategory LastCategory { get; set; } = EnumCatalogCategory.Tree;
}