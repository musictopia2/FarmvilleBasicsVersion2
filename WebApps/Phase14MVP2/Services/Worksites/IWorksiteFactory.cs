using Phase14MVP2.Services.Core;

namespace Phase14MVP2.Services.Worksites;
public interface IWorksiteFactory
{
    WorksiteServicesContext GetWorksiteServices(FarmKey farm);
}