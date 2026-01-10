namespace Phase06IncreaseBarnAndSiloLimits.Services.Crops;
public interface ICropFactory
{
    CropServicesContext GetCropServices(FarmKey farm);
}