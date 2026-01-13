namespace Phase11StoreWithBasicPurchases.Models;
public class InventoryStorageUpgradePlanDocument : IFarmDocument
{
    required public FarmKey Farm { get; init; }
    public BasicList<UpgradeTier> SiloUpgrades { get; init; } = [];
    public BasicList<UpgradeTier> BarnUpgrades { get; init; } = [];
}