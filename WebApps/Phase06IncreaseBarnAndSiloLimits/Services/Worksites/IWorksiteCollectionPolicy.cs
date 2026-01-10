namespace Phase06IncreaseBarnAndSiloLimits.Services.Worksites;
public interface IWorksiteCollectionPolicy
{
    Task<bool> CollectAllAsync();
}