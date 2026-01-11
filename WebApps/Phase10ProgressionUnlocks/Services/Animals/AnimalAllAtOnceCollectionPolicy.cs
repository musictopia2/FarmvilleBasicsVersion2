namespace Phase10ProgressionUnlocks.Services.Animals;
public class AnimalAllAtOnceCollectionPolicy : IAnimalCollectionPolicy
{
    Task<EnumAnimalCollectionMode> IAnimalCollectionPolicy.GetCollectionModeAsync()
    {
        return Task.FromResult(EnumAnimalCollectionMode.AllAtOnce); //a person still has to do but get all instead of doing one by one
    }
}