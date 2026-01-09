using Phase04EnforcingLimits.Services.Core;

namespace Phase04EnforcingLimits.Services.Trees;
public interface ITreeFactory
{
    TreeServicesContext GetTreeServices(FarmKey farm);
}