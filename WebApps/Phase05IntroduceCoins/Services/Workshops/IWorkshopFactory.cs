using Phase05IntroduceCoins.Services.Core;

namespace Phase05IntroduceCoins.Services.Workshops;
public interface IWorkshopFactory
{
    WorkshopServicesContext GetWorkshopServices(FarmKey farm);
}