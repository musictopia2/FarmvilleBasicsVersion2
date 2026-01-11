namespace Phase10ProgressionUnlocks.Services.Crops;
public interface ICropFactory
{
    CropServicesContext GetCropServices(FarmKey farm);
}