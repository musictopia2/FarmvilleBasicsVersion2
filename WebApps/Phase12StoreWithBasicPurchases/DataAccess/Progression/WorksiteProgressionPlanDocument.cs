namespace Phase12StoreWithBasicPurchases.DataAccess.Progression;
public class WorksiteProgressionPlanDocument : IFarmDocument //repeat for others for future understanding.
{
    required public FarmKey Farm { get; set; }
    public BasicList<ItemUnlockRule> UnlockRules { get; set; } = [];
}