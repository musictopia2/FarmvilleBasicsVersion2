namespace Phase10ProgressionUnlocks.Models;
public class AnimalRecipeDocument
{
    public string Animal { get; init; } = "";
    public BasicList<AnimalProductionOption> Options { get; init; } = [];
    required public string Theme { get; init; }
}