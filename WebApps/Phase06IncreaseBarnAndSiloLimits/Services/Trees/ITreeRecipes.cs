namespace Phase06IncreaseBarnAndSiloLimits.Services.Trees;
public interface ITreeRecipes
{
    Task<BasicList<TreeRecipe>> GetTreesAsync();
}