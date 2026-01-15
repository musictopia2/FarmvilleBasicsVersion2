namespace Phase13QuestsBasedOnLevel.DataAccess.Items;
public class ItemPlanDocument : IFarmDocument
{
    public required FarmKey Farm { get; init; }
    public BasicList<ItemPlanModel> ItemList { get; set; } = []; //this is a list of all possible items period.
}