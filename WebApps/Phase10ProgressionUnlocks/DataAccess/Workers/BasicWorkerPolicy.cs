namespace Phase10ProgressionUnlocks.DataAccess.Workers;
public class BasicWorkerPolicy : IWorkerPolicy
{
    Task<bool> IWorkerPolicy.CanLockWorkerAsync(BasicList<WorksiteState> worksites, BasicList<WorkerState> workers, string name)
    {
        return Task.FromResult(false);
    }

    Task<bool> IWorkerPolicy.CanUnlockWorkerAsync(BasicList<WorksiteState> worksites, BasicList<WorkerState> workers, string name)
    {
        return Task.FromResult(false);
    }

    Task IWorkerPolicy.LockWorkerAsync(BasicList<WorksiteState> worksites, BasicList<WorkerState> workers, string name)
    {
        throw new NotImplementedException();
    }

    Task IWorkerPolicy.UnlockWorkerAsync(BasicList<WorksiteState> worksites, BasicList<WorkerState> workers, string name)
    {
        throw new NotImplementedException();
    }
}