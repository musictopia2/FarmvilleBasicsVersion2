namespace Phase12QuestsBasedOnLevel.Models;
public class WorkshopProgressionPlanDocument : IFarmDocument
{
    required public FarmKey Farm { get; set; }
    public BasicList<ItemUnlockRule> UnlockRules { get; set; } = [];
}