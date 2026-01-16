using Phase14MVP2.Services.Core;

namespace Phase14MVP2.Services.Animals;
public interface IAnimalFactory
{
    AnimalServicesContext GetAnimalServices(FarmKey farm);
}