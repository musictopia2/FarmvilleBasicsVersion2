namespace Phase06IncreaseBarnAndSiloLimits.Services.Workers;
public interface IWorkerRegistry
{
    Task<BasicList<WorkerRecipe>> GetWorkersAsync();
}