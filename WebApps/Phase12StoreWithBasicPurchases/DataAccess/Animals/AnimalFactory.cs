namespace Phase12StoreWithBasicPurchases.DataAccess.Animals;
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
            AnimalRegistry = register,
            AnimalRepository = instance,
        };
        return output;
    }
}