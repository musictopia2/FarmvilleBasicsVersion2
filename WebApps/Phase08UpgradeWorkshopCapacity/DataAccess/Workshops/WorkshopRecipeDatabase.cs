using Phase08UpgradeWorkshopCapacity.Services.Core;

namespace Phase08UpgradeWorkshopCapacity.DataAccess.Workshops;
public class WorkshopRecipeDatabase(FarmKey farm) : ListDataAccess<WorkshopRecipeDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IWorkshopRegistry

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorkshopRecipes";
    async Task<BasicList<WorkshopRecipe>> IWorkshopRegistry.GetWorkshopRecipesAsync()
    {
        BasicList<WorkshopRecipeDocument> list = await GetDocumentsAsync();
        BasicList<WorkshopRecipe> output = [];
        list.ForConditionalItems(x => x.Theme == farm.Theme, old =>
        {
            output.Add(new()
            {
                Duration = old.Duration,
                Item = old.Item,
                BuildingName = old.BuildingName,
                Inputs = old.Inputs,
                Output = old.Output
            });
        });
        return output;
    }
}