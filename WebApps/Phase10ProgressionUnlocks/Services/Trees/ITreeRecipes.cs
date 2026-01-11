namespace Phase10ProgressionUnlocks.Services.Trees;
public interface ITreeRecipes
{
    Task<BasicList<TreeRecipe>> GetTreesAsync();
}