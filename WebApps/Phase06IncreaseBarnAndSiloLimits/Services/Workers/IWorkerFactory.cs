using Phase06IncreaseBarnAndSiloLimits.Services.Core;

namespace Phase06IncreaseBarnAndSiloLimits.Services.Workers;
public interface IWorkerFactory
{
    WorkerServicesContext GetWorkerServices(FarmKey farm);
}