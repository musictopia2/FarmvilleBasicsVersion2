using Phase11ProgressionVisibility.Services.Core;

namespace Phase11ProgressionVisibility.Services.Workers;
public interface IWorkerFactory
{
    WorkerServicesContext GetWorkerServices(FarmKey farm);
}