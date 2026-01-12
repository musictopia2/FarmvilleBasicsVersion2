using Phase11ProgressionVisibility.Services.Core;

namespace Phase11ProgressionVisibility.Services.Animals;
public interface IAnimalFactory
{
    AnimalServicesContext GetAnimalServices(FarmKey farm);
}