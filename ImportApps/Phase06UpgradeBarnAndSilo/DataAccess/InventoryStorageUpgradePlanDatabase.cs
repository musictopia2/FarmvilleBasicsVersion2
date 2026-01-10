namespace Phase06UpgradeBarnAndSilo.DataAccess;
public class InventoryStorageUpgradePlanDatabase() : ListDataAccess<InventoryStorageUpgradePlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "InventoryStorageUpgradePlan";
    public async Task ImportAsync(BasicList<InventoryStorageUpgradePlanDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
    public async Task<InventoryStorageUpgradePlanDocument> GetUpgradesAsync(FarmKey farm)
    {
        BasicList<InventoryStorageUpgradePlanDocument> list = await GetDocumentsAsync();
        return list.Single(x => x.Farm.Equals(farm));
    }
}