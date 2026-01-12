namespace Phase10ProgressionUnlocks.DataAccess;
public class CropProgressionPlanDatabase() : ListDataAccess<CropProgressionPlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "CropProgressionPlan";
    public async Task ImportAsync(BasicList<CropProgressionPlanDocument> list)
    {
        await UpsertRecordsAsync(list);
    }

    public async Task<CropProgressionPlanDocument> GetPlanAsync(FarmKey farm)
    {
        var list = await GetDocumentsAsync();
        return list.GetSingleDocument(farm);
    }
}