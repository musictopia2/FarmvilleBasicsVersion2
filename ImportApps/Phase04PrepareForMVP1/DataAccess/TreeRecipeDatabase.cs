namespace Phase04PrepareForMVP1.DataAccess;
internal class TreeRecipeDatabase() : ListDataAccess<TreeRecipeDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "TreeRecipes";
    public async Task ImportAsync(BasicList<TreeRecipeDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
    public async Task<BasicList<TreeRecipeDocument>> GetRecipesAsync()
    {
        return await GetDocumentsAsync();
    }
}