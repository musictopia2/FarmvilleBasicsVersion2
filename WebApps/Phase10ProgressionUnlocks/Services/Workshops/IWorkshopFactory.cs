using Phase10ProgressionUnlocks.Services.Core;

namespace Phase10ProgressionUnlocks.Services.Workshops;
public interface IWorkshopFactory
{
    WorkshopServicesContext GetWorkshopServices(FarmKey farm);
}