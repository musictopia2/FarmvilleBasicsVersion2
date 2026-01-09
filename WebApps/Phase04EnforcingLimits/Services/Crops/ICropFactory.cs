using Phase04EnforcingLimits.Services.Core;

namespace Phase04EnforcingLimits.Services.Crops;
public interface ICropFactory
{
    CropServicesContext GetCropServices(FarmKey farm);
}