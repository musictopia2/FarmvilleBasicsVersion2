namespace Phase09LevelProgression.Services.Workers;
public interface IWorkerPolicy
{
    Task<bool> CanUnlockWorkerAsync(BasicList<WorksiteState> worksites, BasicList<WorkerState> workers, string name);
    Task UnlockWorkerAsync(BasicList<WorksiteState> worksites, BasicList<WorkerState> workers, string name);
    Task<bool> CanLockWorkerAsync(BasicList<WorksiteState> worksites, BasicList<WorkerState> workers, string name);
    Task LockWorkerAsync(BasicList<WorksiteState> worksites, BasicList<WorkerState> workers, string name);
}