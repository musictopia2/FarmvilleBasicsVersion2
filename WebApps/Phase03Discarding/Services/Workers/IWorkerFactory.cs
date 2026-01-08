using Phase03Discarding.Services.Core;

namespace Phase03Discarding.Services.Workers;
public interface IWorkerFactory
{
    WorkerServicesContext GetWorkerServices(FarmKey farm);
}