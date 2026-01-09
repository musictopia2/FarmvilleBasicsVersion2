using Phase04EnforcingLimits.Services.Core;

namespace Phase04EnforcingLimits.Services.Worksites;
public interface IWorksiteFactory
{
    WorksiteServicesContext GetWorksiteServices(FarmKey farm);
}