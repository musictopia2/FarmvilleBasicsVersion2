namespace Phase06IncreaseBarnAndSiloLimits.Services.Animals;
public class AnimalAutomatedCollectionPolicy : IAnimalCollectionPolicy
{
    Task<EnumAnimalCollectionMode> IAnimalCollectionPolicy.GetCollectionModeAsync()
    {
        return Task.FromResult(EnumAnimalCollectionMode.Automated);
    }
}