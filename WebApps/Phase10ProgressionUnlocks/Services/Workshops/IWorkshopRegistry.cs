namespace Phase10ProgressionUnlocks.Services.Workshops;
public interface IWorkshopRegistry
{
    Task<BasicList<WorkshopRecipe>> GetWorkshopRecipesAsync();
}