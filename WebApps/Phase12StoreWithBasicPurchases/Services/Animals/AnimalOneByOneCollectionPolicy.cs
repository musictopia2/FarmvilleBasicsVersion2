namespace Phase12StoreWithBasicPurchases.Services.Animals;
public class AnimalOneByOneCollectionPolicy : IAnimalCollectionPolicy
{
    Task<EnumAnimalCollectionMode> IAnimalCollectionPolicy.GetCollectionModeAsync()
    {
        return Task.FromResult(EnumAnimalCollectionMode.OneAtTime);
    }
}