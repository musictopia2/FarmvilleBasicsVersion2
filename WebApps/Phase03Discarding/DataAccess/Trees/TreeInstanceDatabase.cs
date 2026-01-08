using Phase03Discarding.Services.Core;

namespace Phase03Discarding.DataAccess.Trees;
public class TreeInstanceDatabase(FarmKey farm) : ListDataAccess<TreeInstanceDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, ITreeInstances, ITreePersistence

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "TreeInstances";
    public async Task ImportAsync(BasicList<TreeInstanceDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
    async Task<BasicList<TreeAutoResumeModel>> ITreeInstances.GetTreeInstancesAsync()
    {

        var firsts = await GetDocumentsAsync();
        BasicList<TreeAutoResumeModel> output = firsts.Single(x => x.Farm.Equals(farm)).Trees;
        return output;
    }

    async Task ITreePersistence.SaveTreesAsync(BasicList<TreeAutoResumeModel> trees)
    {
        var list = await GetDocumentsAsync();
        var item = list.Single(x => x.Farm.Equals(farm));
        item.Trees = trees;
        await UpsertRecordsAsync(list);
    }
}