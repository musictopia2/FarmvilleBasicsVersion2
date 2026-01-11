namespace Phase10ProgressionUnlocks.Services.Workers;
public class WorkerServicesContext
{
    required public IWorkerRegistry WorkerRegistry { get; init; }
    required public IWorkerInstances WorkerInstances { get; init; }
    required public IWorkerPolicy WorkerPolicy { get; init; }
}