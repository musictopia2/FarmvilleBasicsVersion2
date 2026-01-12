using Phase10ProgressionUnlocks.Services.Core;

namespace Phase10ProgressionUnlocks.DataAccess.Trees;
public class TreeInstanceDatabase(FarmKey farm) : ListDataAccess<TreeInstanceDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, ITreeRepository

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "TreeInstances";
    async Task<BasicList<TreeAutoResumeModel>> ITreeRepository.LoadAsync()
    {
        var firsts = await GetDocumentsAsync();
        BasicList<TreeAutoResumeModel> output = firsts.GetSingleDocument(farm).Trees;
        return output;
    }

    async Task ITreeRepository.SaveAsync(BasicList<TreeAutoResumeModel> list)
    {
        var firsts = await GetDocumentsAsync();
        var item = firsts.GetSingleDocument(farm);
        item.Trees = list;
        await UpsertRecordsAsync(firsts);
    }

}