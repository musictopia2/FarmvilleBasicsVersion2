using Phase04EnforcingLimits.Services.Core;

namespace Phase04EnforcingLimits.DataAccess.Trees;
internal class TreeRecipeDatabase(FarmKey farm) : ListDataAccess<TreeRecipeDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, ITreeRecipes

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "TreeRecipes";
    async Task<BasicList<TreeRecipe>> ITreeRecipes.GetTreesAsync()
    {
        BasicList<TreeRecipeDocument> list = await GetDocumentsAsync();
        BasicList<TreeRecipe> output = [];
        list.ForConditionalItems(x => x.Theme == farm.Theme, old =>
        {
            output.Add(new()
            {
                TreeName = old.TreeName,
                ProductionTimeForEach = old.ProductionTimeForEach,
                Item = old.Item
            });
        });
        return output;
    }
}