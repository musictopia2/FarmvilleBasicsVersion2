using Phase14MVP2.Services.Core;

namespace Phase14MVP2.Services.Trees;
public interface ITreeFactory
{
    TreeServicesContext GetTreeServices(FarmKey farm);
}