namespace Phase04PrepareForMVP1.DataAccess;
public class AnimalInstanceDatabase() : ListDataAccess<AnimalInstanceDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "AnimalInstances";
    public async Task ImportAsync(BasicList<AnimalInstanceDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
}