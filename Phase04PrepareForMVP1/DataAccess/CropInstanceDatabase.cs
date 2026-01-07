namespace Phase04PrepareForMVP1.DataAccess;
public class CropInstanceDatabase() : ListDataAccess<CropInstanceDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "CropInstances";
    public async Task ImportAsync(BasicList<CropInstanceDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
}