using Phase05IntroduceCoins.Services.Core;

namespace Phase05IntroduceCoins.Services.Trees;
public interface ITreeFactory
{
    TreeServicesContext GetTreeServices(FarmKey farm);
}