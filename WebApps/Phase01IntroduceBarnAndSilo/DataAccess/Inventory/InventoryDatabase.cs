namespace Phase01IntroduceBarnAndSilo.DataAccess.Inventory;
public class InventoryDatabase() : ListDataAccess<InventoryDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IInventoryRepository

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "Inventory";


    async Task<Dictionary<string, int>> IInventoryRepository.LoadAsync(FarmKey farm)
    {
        var list = await GetDocumentsAsync();
        return list.Single(x => x.Farm.Equals(farm)).List;
    }

    async Task IInventoryRepository.SaveAsync(FarmKey farm, Dictionary<string, int> items)
    {
        var list = await GetDocumentsAsync();

        var current = list.Single(x => x.Farm.Equals(farm));
        current.List = items;
        await UpsertRecordsAsync(list);
    }

}