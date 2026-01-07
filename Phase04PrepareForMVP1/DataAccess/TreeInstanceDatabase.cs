namespace Phase04PrepareForMVP1.DataAccess;
public class TreeInstanceDatabase() : ListDataAccess<TreeInstanceDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "TreeInstances";
    public async Task ImportAsync(BasicList<TreeInstanceDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
}