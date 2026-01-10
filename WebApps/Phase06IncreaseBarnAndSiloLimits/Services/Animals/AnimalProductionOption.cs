using Phase06IncreaseBarnAndSiloLimits.Services.Inventory;

namespace Phase06IncreaseBarnAndSiloLimits.Services.Animals;
public class AnimalProductionOption
{
    public ItemAmount Output { get; init; }
    public int Input { get; set; }
    public string Required { get; set; } = "";
    public TimeSpan Duration { get; init; }
}