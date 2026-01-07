namespace Phase01IntroduceBarnAndSilo.Services.Worksites;
public interface IWorksitePersistence
{
    Task SaveWorksitesAsync(BasicList<WorksiteAutoResumeModel> worksites);
}