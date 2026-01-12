namespace Phase10ProgressionUnlocks.DataAccess;

public class TreeProgressionPlanDatabase() : ListDataAccess<TreeProgressionPlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "TreeProgressionPlan";
    public async Task ImportAsync(BasicList<TreeProgressionPlanDocument> list)
    {
        await UpsertRecordsAsync(list);
    }

    public async Task<TreeProgressionPlanDocument> GetPlanAsync(FarmKey farm)
    {
        var list = await GetDocumentsAsync();
        return list.GetSingleDocument(farm);
    }
}