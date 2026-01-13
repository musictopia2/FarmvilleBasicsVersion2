namespace Phase12StoreWithBasicPurchases.Services.Animals;
public class AnimalRecipe
{
    //public string Output { get; init; } = "";

    //public string Required { get; init; } = "";
    public string Animal { get; init; } = "";
    public BasicList<AnimalProductionOption> Options { get; init; } = [];
}