namespace Phase12StoreWithBasicPurchases.DataAccess.Progression;
public class AnimalProgressionPlanDatabase() : ListDataAccess<AnimalProgressionPlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IAnimalProgressionPlanProvider

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "AnimalProgressionPlan";
    public async Task ImportAsync(BasicList<AnimalProgressionPlanDocument> list)
    {
        await UpsertRecordsAsync(list);
    }

    public async Task<AnimalProgressionPlanDocument> GetPlanAsync(FarmKey farm)
    {
        var list = await GetDocumentsAsync();
        return list.GetSingleDocument(farm);
    }

    async Task<BasicList<ItemUnlockRule>> IAnimalProgressionPlanProvider.GetPlanAsync(FarmKey farm)
    {
        var list = await GetDocumentsAsync();
        return list.GetSingleDocument(farm).UnlockRules;
    }
}