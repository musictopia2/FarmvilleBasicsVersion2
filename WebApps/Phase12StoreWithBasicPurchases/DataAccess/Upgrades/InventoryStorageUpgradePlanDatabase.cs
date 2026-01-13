namespace Phase12StoreWithBasicPurchases.DataAccess.Upgrades;
public class InventoryStorageUpgradePlanDatabase() : ListDataAccess<InventoryStorageUpgradePlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration,
    IInventoryStorageUpgradePlanProvider

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "InventoryStorageUpgradePlan";
    async Task<InventoryStorageUpgradePlanModel> IInventoryStorageUpgradePlanProvider.GetPlanAsync(FarmKey farm)
    {
        BasicList<InventoryStorageUpgradePlanDocument> list = await GetDocumentsAsync();

        InventoryStorageUpgradePlanDocument document = list.Single(x => x.Farm.Equals(farm));

        InventoryStorageUpgradePlanModel output = new()
        {
            BarnUpgrades = document.BarnUpgrades,
            SiloUpgrades = document.SiloUpgrades,
        };
        return output;
    }
}