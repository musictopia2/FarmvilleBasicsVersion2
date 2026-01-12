using Phase11ProgressionVisibility.Services.Core;

namespace Phase11ProgressionVisibility.Services.Trees;
public interface ITreeFactory
{
    TreeServicesContext GetTreeServices(FarmKey farm);
}