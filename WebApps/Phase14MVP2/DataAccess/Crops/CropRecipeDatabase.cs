using Phase14MVP2.Services.Core;

namespace Phase14MVP2.DataAccess.Crops;
public class CropRecipeDatabase(FarmKey farm) : ListDataAccess<CropRecipeDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, ICropRegistry
{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "CropRecipes";

    async Task<BasicList<CropRecipe>> ICropRegistry.GetCropsAsync()
    {
        BasicList<CropRecipeDocument> list = await GetDocumentsAsync();
        BasicList<CropRecipe> output = [];
        list.ForConditionalItems(x => x.Theme == farm.Theme, old =>
        {
            output.Add(new()
            {
                Duration = old.Duration,
                Item = old.Item
            });
        });
        return output;
    }
}