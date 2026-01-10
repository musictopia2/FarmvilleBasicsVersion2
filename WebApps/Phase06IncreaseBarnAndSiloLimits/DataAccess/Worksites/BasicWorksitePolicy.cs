namespace Phase06IncreaseBarnAndSiloLimits.DataAccess.Worksites;
public class BasicWorksitePolicy : IWorksiteProgressPolicy
{
    Task<bool> IWorksiteProgressPolicy.CanLockWorksiteAsync(BasicList<WorksiteState> list, string name)
    {
        return Task.FromResult(false);
    }

    Task<bool> IWorksiteProgressPolicy.CanUnlockWorksiteAsync(BasicList<WorksiteState> list, string name)
    {
        return Task.FromResult(false);
    }

    Task<WorksiteState> IWorksiteProgressPolicy.LockWorksiteAsync(BasicList<WorksiteState> list, string name)
    {
        throw new NotImplementedException();
    }

    Task<WorksiteState> IWorksiteProgressPolicy.UnlockWorksiteAsync(BasicList<WorksiteState> list, string name)
    {
        throw new NotImplementedException();
    }
}