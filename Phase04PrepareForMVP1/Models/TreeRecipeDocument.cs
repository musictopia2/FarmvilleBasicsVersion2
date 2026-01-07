namespace Phase04PrepareForMVP1.Models;
public class TreeRecipeDocument
{
    required public string TreeName { get; init; }
    public string Item { get; init; } = ""; //this is what you receive from the tree.
    public TimeSpan ProductionTimeForEach { get; init; }
    required public string Theme { get; init; } //mode is done somewhere else now.
}