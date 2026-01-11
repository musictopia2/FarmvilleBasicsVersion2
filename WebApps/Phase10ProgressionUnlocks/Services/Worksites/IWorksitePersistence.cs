namespace Phase10ProgressionUnlocks.Services.Worksites;
public interface IWorksitePersistence
{
    Task SaveWorksitesAsync(BasicList<WorksiteAutoResumeModel> worksites);
}