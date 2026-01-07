namespace Phase04PrepareForMVP1.DataAccess;
public class QuestInstanceDatabase() : ListDataAccess<QuestDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "QuestInstances";
    public async Task ImportAsync(BasicList<QuestDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
}