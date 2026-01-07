using Phase02BarnAndSiloLimits.Services.Core;

namespace Phase02BarnAndSiloLimits.Services.Workers;
public interface IWorkerFactory
{
    WorkerServicesContext GetWorkerServices(FarmKey farm);
}