namespace Phase02BarnAndSiloLimits.DataAccess.Animals;

public class BasicAnimalPolicy : IAnimalProgressionPolicy
{
    Task<bool> IAnimalProgressionPolicy.CanDecreaseOptionsAsync(BasicList<AnimalState> animals, string animalName)
    {
        return Task.FromResult(false);
    }

    Task<bool> IAnimalProgressionPolicy.CanIncreaseOptionsAsync(BasicList<AnimalState> animals, string animalName)
    {
        return Task.FromResult(false);
    }

    Task<bool> IAnimalProgressionPolicy.CanLockAnimalAsync(BasicList<AnimalState> animals, string animalName)
    {
        return Task.FromResult(false);
    }

    Task<bool> IAnimalProgressionPolicy.CanUnlockAnimalAsync(BasicList<AnimalState> animals, string animalName)
    {
        return Task.FromResult(false);
    }

    Task IAnimalProgressionPolicy.DecreaseOptionsAsync(BasicList<AnimalState> animals, string animalName)
    {
        throw new NotImplementedException();
    }

    Task IAnimalProgressionPolicy.IncreaseOptionsAsync(BasicList<AnimalState> animals, string animalName)
    {
        throw new NotImplementedException();
    }

    Task<AnimalState> IAnimalProgressionPolicy.LockAnimalAsync(BasicList<AnimalState> animals, string animalName)
    {
        throw new NotImplementedException();
    }

    Task<AnimalState> IAnimalProgressionPolicy.UnlockAnimalAsync(BasicList<AnimalState> animals, string animalName)
    {
        throw new NotImplementedException();
    }
}