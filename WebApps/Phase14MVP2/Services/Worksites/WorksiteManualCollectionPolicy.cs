namespace Phase14MVP2.Services.Worksites;
public class WorksiteManualCollectionPolicy : IWorksiteCollectionPolicy
{
    Task<bool> IWorksiteCollectionPolicy.CollectAllAsync()
    {
        return Task.FromResult(false);
    }

}