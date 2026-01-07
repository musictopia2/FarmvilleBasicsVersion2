namespace Phase01IntroduceBarnAndSilo.Services.Worksites;
public interface IWorksiteCollectionPolicy
{
    Task<bool> CollectAllAsync();
}