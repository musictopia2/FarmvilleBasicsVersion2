using Phase06IncreaseBarnAndSiloLimits.Services.Core;

namespace Phase06IncreaseBarnAndSiloLimits.Services.Workshops;
public interface IWorkshopFactory
{
    WorkshopServicesContext GetWorkshopServices(FarmKey farm);
}