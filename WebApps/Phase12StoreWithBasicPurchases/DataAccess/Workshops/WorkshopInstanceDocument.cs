using Phase12StoreWithBasicPurchases.Services.Core;

namespace Phase12StoreWithBasicPurchases.DataAccess.Workshops;
public class WorkshopInstanceDocument : IFarmDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<WorkshopAutoResumeModel> Workshops { get; set; }
}