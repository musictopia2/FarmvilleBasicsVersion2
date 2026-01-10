using Phase08UpgradeWorkshopCapacity.Services.Core;

namespace Phase08UpgradeWorkshopCapacity.DataAccess.Animals;
public class AnimalRecipeDatabase(FarmKey farm) : ListDataAccess<AnimalRecipeDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IAnimalRegistry

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "AnimalRecipes";
    async Task<BasicList<AnimalRecipe>> IAnimalRegistry.GetAnimalsAsync()
    {
        BasicList<AnimalRecipeDocument> list = await GetDocumentsAsync();
        BasicList<AnimalRecipe> output = [];
        list.ForConditionalItems(x => x.Theme == farm.Theme, old =>
        {
            output.Add(new()
            {
                Animal = old.Animal,
                Options = old.Options
            });
        });
        return output;
    }
}