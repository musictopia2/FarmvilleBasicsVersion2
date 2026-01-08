using Phase03Discarding.Services.Core;

namespace Phase03Discarding.Services.Worksites;
public interface IWorksiteFactory
{
    WorksiteServicesContext GetWorksiteServices(FarmKey farm);
}