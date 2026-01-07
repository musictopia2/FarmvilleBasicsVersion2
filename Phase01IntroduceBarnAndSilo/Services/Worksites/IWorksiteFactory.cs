using Phase01IntroduceBarnAndSilo.Services.Core;

namespace Phase01IntroduceBarnAndSilo.Services.Worksites;
public interface IWorksiteFactory
{
    WorksiteServicesContext GetWorksiteServices(FarmKey farm);
}