using Phase02BarnAndSiloLimits.Services.Core;

namespace Phase02BarnAndSiloLimits.Services.Crops;
public interface ICropFactory
{
    CropServicesContext GetCropServices(FarmKey farm);
}