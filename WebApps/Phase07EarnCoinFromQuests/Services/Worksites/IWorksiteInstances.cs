namespace Phase07EarnCoinFromQuests.Services.Worksites;
public interface IWorksiteInstances
{
    Task<BasicList<WorksiteAutoResumeModel>> GetWorksiteInstancesAsync();
}