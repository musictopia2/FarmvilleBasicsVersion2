namespace Phase04PrepareForMVP1.DataAccess;
internal class WorkshopInstanceDatabase() : ListDataAccess<WorkshopInstanceDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorkshopInstances";
    public async Task ImportAsync(BasicList<WorkshopInstanceDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
}