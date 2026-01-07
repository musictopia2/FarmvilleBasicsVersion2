namespace Phase04PrepareForMVP1.DataAccess;
public class WorksiteInstanceDatabase() : ListDataAccess<WorksiteInstanceDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorksiteInstances";
    public async Task ImportAsync(BasicList<WorksiteInstanceDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
}