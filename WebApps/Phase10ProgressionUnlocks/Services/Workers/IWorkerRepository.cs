namespace Phase10ProgressionUnlocks.Services.Workers;
public interface IWorkerRepository
{
    Task<BasicList<WorkerDataModel>> LoadAsync();
    Task SaveAsync(BasicList<WorkerDataModel> data);
}