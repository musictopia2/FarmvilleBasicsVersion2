namespace Phase12QuestsBasedOnLevel.DataAccess;
public class ProgressionProfileDatabase() : ListDataAccess<ProgressionProfileDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "ProgressionProfile";
    public async Task ImportAsync(BasicList<ProgressionProfileDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
    public async Task<ProgressionProfileDocument> GetProfileAsync(FarmKey farm)
    {
        var list = await GetDocumentsAsync();
        return list.GetSingleDocument(farm);
    }
}