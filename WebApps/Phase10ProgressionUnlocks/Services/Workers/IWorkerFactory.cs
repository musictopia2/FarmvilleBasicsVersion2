using Phase10ProgressionUnlocks.Services.Core;

namespace Phase10ProgressionUnlocks.Services.Workers;
public interface IWorkerFactory
{
    WorkerServicesContext GetWorkerServices(FarmKey farm);
}