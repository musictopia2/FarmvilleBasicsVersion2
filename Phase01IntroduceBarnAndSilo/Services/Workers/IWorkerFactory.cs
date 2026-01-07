using Phase01IntroduceBarnAndSilo.Services.Core;

namespace Phase01IntroduceBarnAndSilo.Services.Workers;
public interface IWorkerFactory
{
    WorkerServicesContext GetWorkerServices(FarmKey farm);
}