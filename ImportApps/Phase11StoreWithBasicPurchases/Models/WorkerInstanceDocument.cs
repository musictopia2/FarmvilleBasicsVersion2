namespace Phase11StoreWithBasicPurchases.Models;
public class WorkerInstanceDocument : IFarmDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<UnlockModel> Workers { get; set; } = [];
}
