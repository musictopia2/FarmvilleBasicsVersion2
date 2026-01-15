namespace Phase12QuestsBasedOnLevel.DataAccess;
public class ItemPlanDatabase() : ListDataAccess<ItemPlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "ItemPlan";
    public async Task ImportAsync(BasicList<ItemPlanDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
}