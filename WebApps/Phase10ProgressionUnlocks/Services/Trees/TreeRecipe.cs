namespace Phase10ProgressionUnlocks.Services.Trees;
public class TreeRecipe
{
    required public string TreeName { get; init; }
    public string Item { get; init; } = ""; //this is what you receive from the tree.
    public TimeSpan ProductionTimeForEach { get; init; }
}