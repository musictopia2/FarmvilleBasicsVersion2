namespace Phase05IntroduceCoins.Services.Animals;
public class AnimalOneByOneCollectionPolicy : IAnimalCollectionPolicy
{
    Task<EnumAnimalCollectionMode> IAnimalCollectionPolicy.GetCollectionModeAsync()
    {
        return Task.FromResult(EnumAnimalCollectionMode.OneAtTime);
    }
}