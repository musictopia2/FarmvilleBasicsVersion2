
namespace Phase01IntroduceBarnAndSilo.Services.Worksites;
public class WorksiteAutomatedCollectionPolicy : IWorksiteCollectionPolicy
{
    Task<bool> IWorksiteCollectionPolicy.CollectAllAsync()
    {
        return Task.FromResult(true);
    }

}