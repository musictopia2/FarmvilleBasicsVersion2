using Phase14MVP2.Services.Core;

namespace Phase14MVP2.Services.Workers;
public interface IWorkerFactory
{
    WorkerServicesContext GetWorkerServices(FarmKey farm);
}