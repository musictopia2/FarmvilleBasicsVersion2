using Phase12StoreWithBasicPurchases.Services.Core;

namespace Phase12StoreWithBasicPurchases.Services.Workshops;
public interface IWorkshopFactory
{
    WorkshopServicesContext GetWorkshopServices(FarmKey farm);
}