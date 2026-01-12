namespace Phase11ProgressionVisibility.DataAccess.Progression;
public class TreeProgressionPlanDatabase() : ListDataAccess<TreeProgressionPlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, ITreeProgressionPlanProvider

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "TreeProgressionPlan";
    async Task<BasicList<ItemUnlockRule>> ITreeProgressionPlanProvider.GetPlanAsync(FarmKey farm)
    {
        var list = await GetDocumentsAsync();
        return list.GetSingleDocument(farm).UnlockRules;
    }
}