
namespace Phase13QuestsBasedOnLevel.DataAccess.Items;
public class ItemPlanDatabase() : ListDataAccess<ItemPlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IItemPlanProvider

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "ItemPlan";
    

    async Task<BasicList<ItemPlanModel>> IItemPlanProvider.GetPlanAsync(FarmKey farm)
    {
        var list = await GetDocumentsAsync();
        return list.GetSingleDocument(farm).ItemList;
    }
}