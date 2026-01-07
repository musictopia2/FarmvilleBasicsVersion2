namespace Phase04PrepareForMVP1.DataAccess;
public class AnimalRecipeDatabase() : ListDataAccess<AnimalRecipeDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "AnimalRecipes";
    public async Task ImportAsync(BasicList<AnimalRecipeDocument> list)
    {
        await UpsertRecordsAsync(list);
    }
    public async Task<BasicList<AnimalRecipeDocument>> GetRecipesAsync()
    {
        return await GetDocumentsAsync();
    }
}