using Phase09LevelProgression.Services.Core;

namespace Phase09LevelProgression.Services.Animals;
public interface IAnimalFactory
{
    AnimalServicesContext GetAnimalServices(FarmKey farm);
}