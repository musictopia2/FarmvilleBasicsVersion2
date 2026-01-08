using Phase03Discarding.Services.Core;

namespace Phase03Discarding.Services.Crops;
public interface ICropFactory
{
    CropServicesContext GetCropServices(FarmKey farm);
}