namespace Phase13QuestsBasedOnLevel.Services.Crops;
public interface ICropFactory
{
    CropServicesContext GetCropServices(FarmKey farm);
}