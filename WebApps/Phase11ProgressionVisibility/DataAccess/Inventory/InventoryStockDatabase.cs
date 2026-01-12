namespace Phase11ProgressionVisibility.DataAccess.Inventory;
public class InventoryStockDatabase() : ListDataAccess<InventoryStockDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IInventoryRepository

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "InventoryStock";


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