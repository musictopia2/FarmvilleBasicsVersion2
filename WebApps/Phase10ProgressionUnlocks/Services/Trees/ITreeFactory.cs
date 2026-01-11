using Phase10ProgressionUnlocks.Services.Core;

namespace Phase10ProgressionUnlocks.Services.Trees;
public interface ITreeFactory
{
    TreeServicesContext GetTreeServices(FarmKey farm);
}