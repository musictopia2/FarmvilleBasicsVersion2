using Phase13QuestsBasedOnLevel.Services.Core;

namespace Phase13QuestsBasedOnLevel.Services.Animals;
public interface IAnimalFactory
{
    AnimalServicesContext GetAnimalServices(FarmKey farm);
}