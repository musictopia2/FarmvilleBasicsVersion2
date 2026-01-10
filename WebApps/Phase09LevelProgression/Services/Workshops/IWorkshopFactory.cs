using Phase09LevelProgression.Services.Core;

namespace Phase09LevelProgression.Services.Workshops;
public interface IWorkshopFactory
{
    WorkshopServicesContext GetWorkshopServices(FarmKey farm);
}