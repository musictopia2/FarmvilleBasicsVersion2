namespace Phase13QuestsBasedOnLevel.DataAccess.Progression;
public class CropProgressionPlanDatabase() : ListDataAccess<CropProgressionPlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, ICropProgressionPlanProvider

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
    async Task<CropProgressionPlanModel> ICropProgressionPlanProvider.GetPlanAsync(FarmKey farm)
    {
        var list = await GetDocumentsAsync();
        var item = list.GetSingleDocument(farm);
        CropProgressionPlanModel output = new()
        {
            SlotLevelRequired = item.SlotLevelRequired,
            UnlockRules = item.UnlockRules,
        };
        return output;
    }
}