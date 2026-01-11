namespace Phase10ProgressionUnlocks.Services.Animals;
public interface IAnimalCollectionPolicy
{
    Task<EnumAnimalCollectionMode> GetCollectionModeAsync();
}