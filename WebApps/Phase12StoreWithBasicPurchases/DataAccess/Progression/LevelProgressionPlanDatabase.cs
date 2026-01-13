namespace Phase12StoreWithBasicPurchases.DataAccess.Progression;
public class LevelProgressionPlanDatabase() : ListDataAccess<LevelProgressionPlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration,
    ILevelProgressionPlanProvider

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "LevelProgressionPlan";
    async Task<LevelProgressionPlanModel> ILevelProgressionPlanProvider.GetPlanAsync(FarmKey farm)
    {
        BasicList<LevelProgressionPlanDocument> list = await GetDocumentsAsync();
        LevelProgressionPlanDocument document = list.Single(x => x.Farm.Equals(farm));
        LevelProgressionPlanModel output = new()
        {
            IsEndless = document.IsEndless,
            Tiers = document.Tiers
        };
        return output;
    }
}