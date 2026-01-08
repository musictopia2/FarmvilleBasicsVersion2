using Phase03Discarding.Services.Core;

namespace Phase03Discarding.Services.Animals;
public interface IAnimalFactory
{
    AnimalServicesContext GetAnimalServices(FarmKey farm);
}