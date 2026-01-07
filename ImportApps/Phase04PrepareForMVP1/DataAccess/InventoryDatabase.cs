namespace Phase04PrepareForMVP1.DataAccess;
public class InventoryDatabase() : ListDataAccess<InventoryDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath), 
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "Inventory";
    //hopefully will work.
    //its okay if it wipes out the previos records for this project anyways.
    public async Task ImportAsync(BasicList<InventoryDocument> list)
    {
        await UpsertRecordsAsync(list);
    }

}