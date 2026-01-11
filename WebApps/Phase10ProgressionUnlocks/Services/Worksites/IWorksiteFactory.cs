using Phase10ProgressionUnlocks.Services.Core;

namespace Phase10ProgressionUnlocks.Services.Worksites;
public interface IWorksiteFactory
{
    WorksiteServicesContext GetWorksiteServices(FarmKey farm);
}