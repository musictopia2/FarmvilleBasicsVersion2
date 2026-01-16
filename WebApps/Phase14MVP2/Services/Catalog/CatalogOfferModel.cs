namespace Phase14MVP2.Services.Catalog;
public class CatalogOfferModel
{
    public EnumCatalogCategory Category { get; init; }
    public string TargetName { get; init; } = "";
    public int LevelRequired { get; set; }
    public Dictionary<string, int> Costs { get; set; } = [];
}