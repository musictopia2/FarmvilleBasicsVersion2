namespace Phase07EarnCoinFromQuests.Services.Worksites;
public interface IWorksitePersistence
{
    Task SaveWorksitesAsync(BasicList<WorksiteAutoResumeModel> worksites);
}