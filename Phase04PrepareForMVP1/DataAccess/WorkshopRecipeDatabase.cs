namespace Phase04PrepareForMVP1.DataAccess;
public class WorkshopRecipeDatabase() : ListDataAccess<WorkshopRecipeDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorkshopRecipes";
    public async Task ImportAsync(BasicList<WorkshopRecipeDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
    public async Task<BasicList<WorkshopRecipeDocument>> GetRecipesAsync()
    {
        return await GetDocumentsAsync();
    }
}