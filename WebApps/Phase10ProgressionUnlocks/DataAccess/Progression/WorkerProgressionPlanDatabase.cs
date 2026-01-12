namespace Phase10ProgressionUnlocks.DataAccess.Progression;

public class WorkerProgressionPlanDatabase() : ListDataAccess<WorkerProgressionPlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IWorkerProgressionPlanProvider

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorkerProgressionPlan";
    async Task<BasicList<ItemUnlockRule>> IWorkerProgressionPlanProvider.GetPlanAsync(FarmKey farm)
    {
        var list = await GetDocumentsAsync();
        return list.GetSingleDocument(farm).UnlockRules;
    }
}