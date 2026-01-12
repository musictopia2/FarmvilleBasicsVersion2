using Phase11ProgressionVisibility.Services.Core;

namespace Phase11ProgressionVisibility.Services.Worksites;
public interface IWorksiteFactory
{
    WorksiteServicesContext GetWorksiteServices(FarmKey farm);
}