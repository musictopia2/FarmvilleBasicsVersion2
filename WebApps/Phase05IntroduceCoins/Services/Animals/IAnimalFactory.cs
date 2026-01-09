using Phase05IntroduceCoins.Services.Core;

namespace Phase05IntroduceCoins.Services.Animals;
public interface IAnimalFactory
{
    AnimalServicesContext GetAnimalServices(FarmKey farm);
}