using Phase08UpgradeWorkshopCapacity.Services.Inventory;

namespace Phase08UpgradeWorkshopCapacity.Services.Animals;
public class AnimalProductionOption
{
    public ItemAmount Output { get; init; }
    public int Input { get; set; }
    public string Required { get; set; } = "";
    public TimeSpan Duration { get; init; }
}