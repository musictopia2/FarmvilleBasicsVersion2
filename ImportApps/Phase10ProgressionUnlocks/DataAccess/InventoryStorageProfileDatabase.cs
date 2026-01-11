namespace Phase10ProgressionUnlocks.DataAccess;
public class InventoryStorageProfileDatabase() : ListDataAccess<InventoryStorageProfileDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "InventoryStorageProfile";
    public async Task ImportAsync(BasicList<InventoryStorageProfileDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
}