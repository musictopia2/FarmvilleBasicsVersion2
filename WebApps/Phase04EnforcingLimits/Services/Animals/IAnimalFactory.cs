using Phase04EnforcingLimits.Services.Core;

namespace Phase04EnforcingLimits.Services.Animals;
public interface IAnimalFactory
{
    AnimalServicesContext GetAnimalServices(FarmKey farm);
}