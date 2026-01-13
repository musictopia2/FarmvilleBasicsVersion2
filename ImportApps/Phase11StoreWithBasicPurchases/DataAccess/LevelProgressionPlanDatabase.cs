namespace Phase11StoreWithBasicPurchases.DataAccess;
public class LevelProgressionPlanDatabase() : ListDataAccess<LevelProgressionPlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "LevelProgressionPlan";
    public async Task ImportAsync(BasicList<LevelProgressionPlanDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
    
}