using Phase05IntroduceCoins.Services.Core;

namespace Phase05IntroduceCoins.Services.Worksites;
public interface IWorksiteFactory
{
    WorksiteServicesContext GetWorksiteServices(FarmKey farm);
}