using Phase03Discarding.Services.Core;

namespace Phase03Discarding.Services.Workshops;
public interface IWorkshopFactory
{
    WorkshopServicesContext GetWorkshopServices(FarmKey farm);
}