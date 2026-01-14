namespace Phase12QuestsBasedOnLevel.Models;
public class CatalogOfferModel
{
    public EnumCatalogCategory Category { get; init; }
    public string TargetName { get; init; } = "";
    public int LevelRequired { get; set; }
    public Dictionary<string, int> Costs { get; set; } = [];
}