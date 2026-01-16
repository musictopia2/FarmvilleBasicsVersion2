namespace Phase14MVP2.Services.Worksites;
public interface IWorksiteRepository
{
    Task<BasicList<WorksiteAutoResumeModel>> LoadAsync();
    Task SaveAsync(BasicList<WorksiteAutoResumeModel> worksites);
}