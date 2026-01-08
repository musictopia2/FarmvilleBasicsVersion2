using Phase03Discarding.Services.Core;

namespace Phase03Discarding.Services.Trees;
public interface ITreeFactory
{
    TreeServicesContext GetTreeServices(FarmKey farm);
}