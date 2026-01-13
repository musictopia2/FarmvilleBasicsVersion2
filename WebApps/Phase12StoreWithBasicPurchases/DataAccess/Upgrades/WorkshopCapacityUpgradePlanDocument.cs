namespace Phase12StoreWithBasicPurchases.DataAccess.Upgrades;
public class WorkshopCapacityUpgradePlanDocument
{
    required public FarmKey Farm { get; init; }
    required public string WorkshopName { get; init; }
    //for now, only capacity.  if it makes sense to have all upgrades here can do and rename (?)
    required public BasicList<UpgradeTier> Upgrades { get; init; }
    //for this one, the new level is one higher than the previous one.
}