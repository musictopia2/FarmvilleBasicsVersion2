using Phase05IntroduceCoins.Services.Inventory;

namespace Phase05IntroduceCoins.Services.Animals;
public class AnimalProductionOption
{
    public ItemAmount Output { get; init; }
    public int Input { get; set; }
    public string Required { get; set; } = "";
    public TimeSpan Duration { get; init; }
}