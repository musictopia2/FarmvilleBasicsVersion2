namespace Phase12StoreWithBasicPurchases.Services.Crops;
public class CropServicesContext
{
    required public ICropRegistry CropRegistry{ get; init; }
    required public ICropRepository CropRepository { get; init; }
    required public ICropHarvestPolicy CropHarvestPolicy { get; init; }
}