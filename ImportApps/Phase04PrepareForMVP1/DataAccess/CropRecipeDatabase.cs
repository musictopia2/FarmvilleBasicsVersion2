namespace Phase04PrepareForMVP1.DataAccess;
public class CropRecipeDatabase() : ListDataAccess<CropRecipeDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration
{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "CropRecipes";
    public async Task ImportAsync(BasicList<CropRecipeDocument> list)
    {
        await UpsertRecordsAsync(list);
    }

    public async Task<BasicList<CropRecipeDocument>> GetRecipesAsync()
    {
        var list = await GetDocumentsAsync();
        return list;
    }

}