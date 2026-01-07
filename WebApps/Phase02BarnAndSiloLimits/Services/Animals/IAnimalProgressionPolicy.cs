namespace Phase02BarnAndSiloLimits.Services.Animals;
public interface IAnimalProgressionPolicy
{
    Task<bool> CanUnlockAnimalAsync(BasicList<AnimalState> animals, string animalName);

    Task<AnimalState> UnlockAnimalAsync(BasicList<AnimalState> animals, string animalName);
    Task<bool> CanLockAnimalAsync(BasicList<AnimalState> animals, string animalName);
    Task<AnimalState> LockAnimalAsync(BasicList<AnimalState> animals, string animalName);

    Task<bool> CanIncreaseOptionsAsync(BasicList<AnimalState> animals, string animalName);
    Task IncreaseOptionsAsync(BasicList<AnimalState> animals, string animalName);
    Task<bool> CanDecreaseOptionsAsync(BasicList<AnimalState> animals, string animalName);
    Task DecreaseOptionsAsync(BasicList<AnimalState> animals, string animalName);
}