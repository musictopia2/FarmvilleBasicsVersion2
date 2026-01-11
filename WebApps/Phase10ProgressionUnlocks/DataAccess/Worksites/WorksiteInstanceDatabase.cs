using Phase10ProgressionUnlocks.Services.Core;

namespace Phase10ProgressionUnlocks.DataAccess.Worksites;
public class WorksiteInstanceDatabase(FarmKey farm) : ListDataAccess<WorksiteInstanceDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IWorksiteInstances, IWorksitePersistence
{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorksiteInstances";
    async Task<BasicList<WorksiteAutoResumeModel>> IWorksiteInstances.GetWorksiteInstancesAsync()
    {
        var firsts = await GetDocumentsAsync();
        BasicList<WorksiteAutoResumeModel> output = firsts.Single(x => x.Farm.Equals(farm)).Worksites;
        return output;
    }
    async Task IWorksitePersistence.SaveWorksitesAsync(BasicList<WorksiteAutoResumeModel> worksites)
    {
        var list = await GetDocumentsAsync();
        var item = list.Single(x => x.Farm.Equals(farm));
        item.Worksites = worksites;
        await UpsertRecordsAsync(list);
    }
}