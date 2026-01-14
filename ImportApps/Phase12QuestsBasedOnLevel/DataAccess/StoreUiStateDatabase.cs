namespace Phase12QuestsBasedOnLevel.DataAccess;
public class StoreUiStateDatabase() : ListDataAccess<StoreUiStateDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "StoreUiState";
    public async Task ImportAsync(BasicList<StoreUiStateDocument> list)
    {
        await UpsertRecordsAsync(list);
    }

}