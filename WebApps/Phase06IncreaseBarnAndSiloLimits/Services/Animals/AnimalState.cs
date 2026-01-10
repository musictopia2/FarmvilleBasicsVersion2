namespace Phase06IncreaseBarnAndSiloLimits.Services.Animals;
public class AnimalState : AnimalView
{
    public bool Unlocked { get; set; }
    public bool InProgress { get; set; }

    public int TotalAllowedOptions { get; set; }
    public int TotalPossibleOptions { get; set; }

    //here is where if i need extra information so policies can make decisions, will be here.
}