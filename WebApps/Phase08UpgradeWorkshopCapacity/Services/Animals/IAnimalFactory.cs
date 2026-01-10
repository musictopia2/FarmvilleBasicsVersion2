using Phase08UpgradeWorkshopCapacity.Services.Core;

namespace Phase08UpgradeWorkshopCapacity.Services.Animals;
public interface IAnimalFactory
{
    AnimalServicesContext GetAnimalServices(FarmKey farm);
}