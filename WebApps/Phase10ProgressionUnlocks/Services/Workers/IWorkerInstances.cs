namespace Phase10ProgressionUnlocks.Services.Workers;
public interface IWorkerInstances
{
    Task<BasicList<WorkerDataModel>> GetWorkerInstancesAsync();
}