namespace Phase11StoreWithBasicPurchases.DataAccess;

public class WorkerProgressionPlanDatabase() : ListDataAccess<WorkerProgressionPlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorkerProgressionPlan";
    public async Task ImportAsync(BasicList<WorkerProgressionPlanDocument> list)
    {
        await UpsertRecordsAsync(list);
    }

    public async Task<WorkerProgressionPlanDocument> GetPlanAsync(FarmKey farm)
    {
        var list = await GetDocumentsAsync();
        return list.GetSingleDocument(farm);
    }
}