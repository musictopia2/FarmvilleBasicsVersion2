namespace Phase06IncreaseBarnAndSiloLimits.Services.Upgrades;
public interface IUpgradeFactory
{
    UpgradeServicesContext GetUpgradeServices(FarmKey farm);
}
