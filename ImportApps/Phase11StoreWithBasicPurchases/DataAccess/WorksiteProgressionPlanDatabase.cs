namespace Phase11StoreWithBasicPurchases.DataAccess;
public class WorksiteProgressionPlanDatabase() : ListDataAccess<WorksiteProgressionPlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorksiteProgressionPlan";
    public async Task ImportAsync(BasicList<WorksiteProgressionPlanDocument> list)
    {
        await UpsertRecordsAsync(list);
    }

    public async Task<WorksiteProgressionPlanDocument> GetPlanAsync(FarmKey farm)
    {
        var list = await GetDocumentsAsync();
        return list.GetSingleDocument(farm);
    }
}