using Phase14MVP2.Services.Core;

namespace Phase14MVP2.Services.Workshops;
public interface IWorkshopFactory
{
    WorkshopServicesContext GetWorkshopServices(FarmKey farm);
}