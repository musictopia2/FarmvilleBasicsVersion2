namespace Phase12StoreWithBasicPurchases.Services.Workers;
public class WorkerServicesContext
{
    required public IWorkerRegistry WorkerRegistry { get; init; }
    required public IWorkerRepository WorkerRepository { get; init; }
}