using Phase09LevelProgression.Services.Core;

namespace Phase09LevelProgression.Services.Worksites;
public interface IWorksiteFactory
{
    WorksiteServicesContext GetWorksiteServices(FarmKey farm);
}