namespace Phase10ProgressionUnlocks.DataAccess.Progression;
public class WorkshopProgressionPlanDatabase() : ListDataAccess<WorkshopProgressionPlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IWorkshopProgressionPlanProvider

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorkshopProgressionPlan";
    async Task<BasicList<ItemUnlockRule>> IWorkshopProgressionPlanProvider.GetPlanAsync(FarmKey farm)
    {
        var list = await GetDocumentsAsync();
        return list.GetSingleDocument(farm).UnlockRules;
    }
}