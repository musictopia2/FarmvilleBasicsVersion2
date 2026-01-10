using Phase09LevelProgression.Services.Core;

namespace Phase09LevelProgression.DataAccess.Animals;
public class AnimalFactory : IAnimalFactory
{
    AnimalServicesContext IAnimalFactory.GetAnimalServices(FarmKey farm)
    {
        IAnimalCollectionPolicy collection = new AnimalAllAtOnceCollectionPolicy();
        IAnimalRegistry register;
        register = new AnimalRecipeDatabase(farm);
        AnimalInstanceDatabase instance = new(farm);
        AnimalServicesContext output = new()
        {
            AnimalCollectionPolicy = collection,
            AnimalProgressionPolicy = new BasicAnimalPolicy(),
            AnimalRegistry = register,
            AnimalInstances = instance,
            AnimalPersistence = instance
        };
        return output;
    }
}