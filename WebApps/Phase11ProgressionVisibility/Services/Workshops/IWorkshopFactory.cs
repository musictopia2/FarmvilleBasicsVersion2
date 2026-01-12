using Phase11ProgressionVisibility.Services.Core;

namespace Phase11ProgressionVisibility.Services.Workshops;
public interface IWorkshopFactory
{
    WorkshopServicesContext GetWorkshopServices(FarmKey farm);
}