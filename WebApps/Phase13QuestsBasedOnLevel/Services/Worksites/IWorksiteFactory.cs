using Phase13QuestsBasedOnLevel.Services.Core;

namespace Phase13QuestsBasedOnLevel.Services.Worksites;
public interface IWorksiteFactory
{
    WorksiteServicesContext GetWorksiteServices(FarmKey farm);
}