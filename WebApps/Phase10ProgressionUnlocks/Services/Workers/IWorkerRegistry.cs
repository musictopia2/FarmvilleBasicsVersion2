namespace Phase10ProgressionUnlocks.Services.Workers;
public interface IWorkerRegistry
{
    Task<BasicList<WorkerRecipe>> GetWorkersAsync();
}