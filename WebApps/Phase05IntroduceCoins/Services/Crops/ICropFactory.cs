using Phase05IntroduceCoins.Services.Core;

namespace Phase05IntroduceCoins.Services.Crops;
public interface ICropFactory
{
    CropServicesContext GetCropServices(FarmKey farm);
}