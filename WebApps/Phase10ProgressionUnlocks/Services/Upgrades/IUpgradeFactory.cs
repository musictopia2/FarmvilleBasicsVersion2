namespace Phase10ProgressionUnlocks.Services.Upgrades;
public interface IUpgradeFactory
{
    UpgradeServicesContext GetUpgradeServices(FarmKey farm);
}