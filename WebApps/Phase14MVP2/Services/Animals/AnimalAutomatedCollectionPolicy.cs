namespace Phase14MVP2.Services.Animals;
public class AnimalAutomatedCollectionPolicy : IAnimalCollectionPolicy
{
    Task<EnumAnimalCollectionMode> IAnimalCollectionPolicy.GetCollectionModeAsync()
    {
        return Task.FromResult(EnumAnimalCollectionMode.Automated);
    }
}