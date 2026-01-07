namespace Phase04PrepareForMVP1.DataAccess;
internal class WorkerRecipeDatabase() : ListDataAccess<WorkerRecipeDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorkerRecipes";
    public async Task ImportAsync(BasicList<WorkerRecipeDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
}