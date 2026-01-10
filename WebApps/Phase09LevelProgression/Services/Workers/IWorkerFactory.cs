using Phase09LevelProgression.Services.Core;

namespace Phase09LevelProgression.Services.Workers;
public interface IWorkerFactory
{
    WorkerServicesContext GetWorkerServices(FarmKey farm);
}