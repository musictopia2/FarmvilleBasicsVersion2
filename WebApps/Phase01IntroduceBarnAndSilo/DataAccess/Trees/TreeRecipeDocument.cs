namespace Phase01IntroduceBarnAndSilo.DataAccess.Trees;
public class TreeRecipeDocument
{
    required public string TreeName { get; init; }
    public string Item { get; init; } = ""; //this is what you receive from the tree.
    public TimeSpan ProductionTimeForEach { get; init; }
    required public string Theme { get; init; }
}