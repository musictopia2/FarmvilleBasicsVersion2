using Phase08UpgradeWorkshopCapacity.Services.Core;

namespace Phase08UpgradeWorkshopCapacity.DataAccess.Workers;
internal class WorkerRecipeDatabase(FarmKey farm) : ListDataAccess<WorkerRecipeDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IWorkerRegistry

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorkerRecipes";
    async Task<BasicList<WorkerRecipe>> IWorkerRegistry.GetWorkersAsync()
    {
        BasicList<WorkerRecipeDocument> list = await GetDocumentsAsync();
        BasicList<WorkerRecipe> output = [];
        list.ForConditionalItems(x => x.Theme == farm.Theme, old =>
        {
            output.Add(new()
            {
                WorkerName = old.WorkerName,
                Benefits = old.Benefits,
                CurrentLocation = old.CurrentLocation,
                Details = old.Details,
                LockedLocation = old.LockedLocation,
                WorkerStatus = old.WorkerStatus
            });
        });
        return output;
    }
}