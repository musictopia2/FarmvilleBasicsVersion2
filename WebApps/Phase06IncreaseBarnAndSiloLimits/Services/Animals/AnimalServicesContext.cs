namespace Phase06IncreaseBarnAndSiloLimits.Services.Animals;
public class AnimalServicesContext
{
    required public IAnimalRegistry AnimalRegistry { get; init; }
    required public IAnimalInstances AnimalInstances { get; init; }
    required public IAnimalProgressionPolicy AnimalProgressionPolicy { get; init; }
    required public IAnimalCollectionPolicy AnimalCollectionPolicy { get; init; }
    required public IAnimalPersistence AnimalPersistence { get; init; }
}