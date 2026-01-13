namespace Phase11StoreWithBasicPurchases.DataAccess;
public class WorkerInstanceDatabase() : ListDataAccess<WorkerInstanceDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorkerInstances";
    public async Task ImportAsync(BasicList<WorkerInstanceDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
}