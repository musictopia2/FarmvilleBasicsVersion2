namespace Phase13QuestsBasedOnLevel.Services.Animals;
public class AnimalServicesContext
{
    required public IAnimalRegistry AnimalRegistry { get; init; }
    required public IAnimalRepository AnimalRepository { get; init; }
    required public IAnimalCollectionPolicy AnimalCollectionPolicy { get; init; }
}