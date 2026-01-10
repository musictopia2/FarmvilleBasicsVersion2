using Phase07EarnCoinFromQuests.Services.Core;

namespace Phase07EarnCoinFromQuests.Services.Workers;
public interface IWorkerFactory
{
    WorkerServicesContext GetWorkerServices(FarmKey farm);
}