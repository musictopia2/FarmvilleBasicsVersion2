using Phase12StoreWithBasicPurchases.Services.Core;

namespace Phase12StoreWithBasicPurchases.Services.Worksites;
public interface IWorksiteFactory
{
    WorksiteServicesContext GetWorksiteServices(FarmKey farm);
}