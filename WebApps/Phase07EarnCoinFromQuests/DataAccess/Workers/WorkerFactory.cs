using Phase07EarnCoinFromQuests.Services.Core;

namespace Phase07EarnCoinFromQuests.DataAccess.Workers;
public class WorkerFactory : IWorkerFactory
{
    WorkerServicesContext IWorkerFactory.GetWorkerServices(FarmKey farm)
    {
        
        IWorkerRegistry register;
        register = new WorkerRecipeDatabase(farm);
        WorkerServicesContext output = new()
        {
            WorkerPolicy = new BasicWorkerPolicy(),
            WorkerRegistry = register,
            WorkerInstances = new BasicWorkerInstances(register)
        };
        return output;
    }
}