namespace Phase01IntroduceBarnAndSilo.DataAccess.Animals;
public class AnimalRecipeDocument
{
    public string Animal { get; init; } = "";
    public BasicList<AnimalProductionOption> Options { get; init; } = [];
    required public string Theme { get; init; }
}