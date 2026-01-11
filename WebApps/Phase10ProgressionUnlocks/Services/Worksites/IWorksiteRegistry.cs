namespace Phase10ProgressionUnlocks.Services.Worksites;
public interface IWorksiteRegistry
{
    Task<BasicList<WorksiteRecipe>> GetWorksitesAsync();
}