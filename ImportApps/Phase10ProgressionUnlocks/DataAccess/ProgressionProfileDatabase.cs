namespace Phase10ProgressionUnlocks.DataAccess;
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
}