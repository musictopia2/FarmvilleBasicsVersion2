namespace Phase11StoreWithBasicPurchases.Models;
public class WorksiteProgressionPlanDocument : IFarmDocument //repeat for others for future understanding.
{
    required public FarmKey Farm { get; set; }
    public BasicList<ItemUnlockRule> UnlockRules { get; set; } = [];
}