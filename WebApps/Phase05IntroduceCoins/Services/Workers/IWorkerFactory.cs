using Phase05IntroduceCoins.Services.Core;

namespace Phase05IntroduceCoins.Services.Workers;
public interface IWorkerFactory
{
    WorkerServicesContext GetWorkerServices(FarmKey farm);
}