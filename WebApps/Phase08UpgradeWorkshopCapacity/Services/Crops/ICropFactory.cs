namespace Phase08UpgradeWorkshopCapacity.Services.Crops;
public interface ICropFactory
{
    CropServicesContext GetCropServices(FarmKey farm);
}