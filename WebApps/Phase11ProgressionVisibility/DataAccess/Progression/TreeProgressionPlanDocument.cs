namespace Phase11ProgressionVisibility.DataAccess.Progression;
public class TreeProgressionPlanDocument : IFarmDocument
{
    required public FarmKey Farm { get; set; }
    public BasicList<ItemUnlockRule> UnlockRules { get; set; } = [];
}