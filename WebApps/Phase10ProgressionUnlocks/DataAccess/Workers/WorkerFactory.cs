using Phase10ProgressionUnlocks.Services.Core;

namespace Phase10ProgressionUnlocks.DataAccess.Workers;
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