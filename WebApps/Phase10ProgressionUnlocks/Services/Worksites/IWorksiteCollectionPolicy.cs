namespace Phase10ProgressionUnlocks.Services.Worksites;
public interface IWorksiteCollectionPolicy
{
    Task<bool> CollectAllAsync();
}