namespace Phase11ProgressionVisibility.DataAccess.Progression;
public class WorksiteProgressionPlanDatabase() : ListDataAccess<WorksiteProgressionPlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IWorksiteProgressionPlanProvider

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorksiteProgressionPlan";
    async Task<BasicList<ItemUnlockRule>> IWorksiteProgressionPlanProvider.GetPlanAsync(FarmKey farm)
    {
        var list = await GetDocumentsAsync();
        return list.GetSingleDocument(farm).UnlockRules;
    }
}