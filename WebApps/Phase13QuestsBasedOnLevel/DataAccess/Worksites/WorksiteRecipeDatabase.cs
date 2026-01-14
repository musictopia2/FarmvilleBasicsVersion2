using Phase13QuestsBasedOnLevel.Services.Core;

namespace Phase13QuestsBasedOnLevel.DataAccess.Worksites;
public class WorksiteRecipeDatabase(FarmKey farm) : ListDataAccess<WorksiteRecipeDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IWorksiteRegistry

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorksiteRecipes";
    async Task<BasicList<WorksiteRecipe>> IWorksiteRegistry.GetWorksitesAsync()
    {
        BasicList<WorksiteRecipeDocument> list = await GetDocumentsAsync();
        BasicList<WorksiteRecipe> output = [];
        list.ForConditionalItems(x => x.Theme == farm.Theme, old =>
        {
            output.Add(new()
            {
                BaselineBenefits = old.BaselineBenefits,
                StartText = old.StartText,
                Duration = old.Duration,
                Inputs = old.Inputs,
                Location = old.Location,
                MaximumWorkers = old.MaximumWorkers
            });
        });
        return output;
    }
}