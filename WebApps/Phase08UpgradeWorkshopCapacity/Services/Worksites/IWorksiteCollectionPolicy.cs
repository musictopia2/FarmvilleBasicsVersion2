namespace Phase08UpgradeWorkshopCapacity.Services.Worksites;
public interface IWorksiteCollectionPolicy
{
    Task<bool> CollectAllAsync();
}