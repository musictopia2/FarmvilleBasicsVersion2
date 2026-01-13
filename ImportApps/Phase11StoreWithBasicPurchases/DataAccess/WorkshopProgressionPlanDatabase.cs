namespace Phase11StoreWithBasicPurchases.DataAccess;

public class WorkshopProgressionPlanDatabase() : ListDataAccess<WorkshopProgressionPlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorkshopProgressionPlan";
    public async Task ImportAsync(BasicList<WorkshopProgressionPlanDocument> list)
    {
        await UpsertRecordsAsync(list);
    }

    public async Task<WorkshopProgressionPlanDocument> GetPlanAsync(FarmKey farm)
    {
        var list = await GetDocumentsAsync();
        return list.GetSingleDocument(farm);
    }
}