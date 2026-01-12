namespace Phase10ProgressionUnlocks.DataAccess.Worksites;

public class WorksiteInstanceDatabase(FarmKey farm) : ListDataAccess<WorksiteInstanceDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IWorksiteRepository
{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorksiteInstances";
    async Task<BasicList<WorksiteAutoResumeModel>> IWorksiteRepository.LoadAsync()
    {
        var firsts = await GetDocumentsAsync();
        BasicList<WorksiteAutoResumeModel> output = firsts.GetSingleDocument(farm).Worksites;
        return output;
    }

    async Task IWorksiteRepository.SaveAsync(BasicList<WorksiteAutoResumeModel> worksites)
    {
        var list = await GetDocumentsAsync();
        var item = list.GetSingleDocument(farm);
        item.Worksites = worksites;
        await UpsertRecordsAsync(list);
    }

}