using Phase10ProgressionUnlocks.Services.Core;

namespace Phase10ProgressionUnlocks.Services.Animals;
public interface IAnimalFactory
{
    AnimalServicesContext GetAnimalServices(FarmKey farm);
}