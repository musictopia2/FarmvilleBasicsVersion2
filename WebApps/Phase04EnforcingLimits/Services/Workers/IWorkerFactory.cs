using Phase04EnforcingLimits.Services.Core;

namespace Phase04EnforcingLimits.Services.Workers;
public interface IWorkerFactory
{
    WorkerServicesContext GetWorkerServices(FarmKey farm);
}