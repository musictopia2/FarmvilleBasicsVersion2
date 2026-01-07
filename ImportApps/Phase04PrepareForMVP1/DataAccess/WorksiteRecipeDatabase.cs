namespace Phase04PrepareForMVP1.DataAccess;
public class WorksiteRecipeDatabase() : ListDataAccess<WorksiteRecipeDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorksiteRecipes";
    public async Task ImportAsync(BasicList<WorksiteRecipeDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
    public async Task<BasicList<WorksiteRecipeDocument>> GetRecipesAsync()
    {
        return await GetDocumentsAsync();
    }
}