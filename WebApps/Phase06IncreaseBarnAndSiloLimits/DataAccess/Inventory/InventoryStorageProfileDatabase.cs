namespace Phase06IncreaseBarnAndSiloLimits.DataAccess.Inventory;
public class InventoryStorageProfileDatabase(FarmKey farm) : ListDataAccess<InventoryStorageProfileDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration,
    IInventoryProfile

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "InventoryStorageProfile";
    async Task<InventoryStorageProfileModel> IInventoryProfile.LoadAsync()
    {
        var list = await GetDocumentsAsync();

        var item = list.Single(x => x.Farm.Equals(farm));
        return new()
        {
            BarnSize = item.BarnSize,
            SiloSize = item.SiloSize,
        };
    }
    async Task IInventoryProfile.SaveAsync(InventoryStorageProfileModel profile)
    {
        var list = await GetDocumentsAsync();

        var current = list.Single(x => x.Farm.Equals(farm));
        current.SiloSize = profile.SiloSize;
        current.BarnSize = profile.BarnSize;
        await UpsertRecordsAsync(list);
    }
}