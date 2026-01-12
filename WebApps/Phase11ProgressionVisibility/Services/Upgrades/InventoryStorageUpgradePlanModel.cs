namespace Phase11ProgressionVisibility.Services.Upgrades;
public class InventoryStorageUpgradePlanModel
{
    public BasicList<UpgradeTier> SiloUpgrades { get; init; } = [];
    public BasicList<UpgradeTier> BarnUpgrades { get; init; } = [];
}