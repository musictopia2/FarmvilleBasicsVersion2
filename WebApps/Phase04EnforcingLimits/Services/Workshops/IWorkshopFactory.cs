using Phase04EnforcingLimits.Services.Core;

namespace Phase04EnforcingLimits.Services.Workshops;
public interface IWorkshopFactory
{
    WorkshopServicesContext GetWorkshopServices(FarmKey farm);
}