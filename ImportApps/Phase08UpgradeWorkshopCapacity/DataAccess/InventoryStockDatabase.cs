namespace Phase08UpgradeWorkshopCapacity.DataAccess;
public class InventoryStockDatabase() : ListDataAccess<InventoryStockDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath), 
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "InventoryStock";
    //hopefully will work.
    //its okay if it wipes out the previos records for this project anyways.
    public async Task ImportAsync(BasicList<InventoryStockDocument> list)
    {
        await UpsertRecordsAsync(list);
    }

}