using Phase07EarnCoinFromQuests.Services.Core;

namespace Phase07EarnCoinFromQuests.Services.Trees;
public interface ITreeFactory
{
    TreeServicesContext GetTreeServices(FarmKey farm);
}