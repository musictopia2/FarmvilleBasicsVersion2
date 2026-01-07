namespace Phase04PrepareForMVP1.DataAccess;

public class BalanceProfileDatabase() : ListDataAccess<BalanceProfileDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "BalanceCraftingTimes";
    public async Task ImportAsync(BasicList<BalanceProfileDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
}