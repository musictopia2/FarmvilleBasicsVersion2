namespace Phase06IncreaseBarnAndSiloLimits.Services.Worksites;
public interface IWorksiteProgressPolicy
{
    Task<bool> CanUnlockWorksiteAsync(BasicList<WorksiteState> list, string name);
    Task<WorksiteState> UnlockWorksiteAsync(BasicList<WorksiteState> list, string name);
    Task<bool> CanLockWorksiteAsync(BasicList<WorksiteState> list, string name);
    Task<WorksiteState> LockWorksiteAsync(BasicList<WorksiteState> list, string name);
}