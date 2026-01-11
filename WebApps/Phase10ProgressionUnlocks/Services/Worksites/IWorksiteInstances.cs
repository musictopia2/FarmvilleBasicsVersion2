namespace Phase10ProgressionUnlocks.Services.Worksites;
public interface IWorksiteInstances
{
    Task<BasicList<WorksiteAutoResumeModel>> GetWorksiteInstancesAsync();
}