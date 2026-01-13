namespace Phase12StoreWithBasicPurchases.Services.Crops;
public interface ICropFactory
{
    CropServicesContext GetCropServices(FarmKey farm);
}