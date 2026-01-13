namespace Phase11StoreWithBasicPurchases.DataAccess;
public class WorkshopCapacityUpgradePlanDatabase() : ListDataAccess<WorkshopCapacityUpgradePlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorkshopCapacityUpgradePlan";
    public async Task ImportAsync(BasicList<WorkshopCapacityUpgradePlanDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
    public async Task<BasicList<WorkshopCapacityUpgradePlanDocument>> GetUpgradesAsync(FarmKey farm)
    {
        BasicList<WorkshopCapacityUpgradePlanDocument> list = await GetDocumentsAsync();
        return list.GetDocuments(farm);
    }
}