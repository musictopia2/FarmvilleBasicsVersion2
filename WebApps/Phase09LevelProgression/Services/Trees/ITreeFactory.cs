using Phase09LevelProgression.Services.Core;

namespace Phase09LevelProgression.Services.Trees;
public interface ITreeFactory
{
    TreeServicesContext GetTreeServices(FarmKey farm);
}