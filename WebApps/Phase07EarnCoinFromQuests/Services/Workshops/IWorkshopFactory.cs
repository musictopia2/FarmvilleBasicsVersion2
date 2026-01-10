using Phase07EarnCoinFromQuests.Services.Core;

namespace Phase07EarnCoinFromQuests.Services.Workshops;
public interface IWorkshopFactory
{
    WorkshopServicesContext GetWorkshopServices(FarmKey farm);
}