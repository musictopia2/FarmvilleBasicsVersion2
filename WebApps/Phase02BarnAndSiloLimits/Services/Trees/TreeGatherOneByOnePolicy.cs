
namespace Phase02BarnAndSiloLimits.Services.Trees;
public class TreeGatherOneByOnePolicy : ITreeGatheringPolicy
{
    Task<bool> ITreeGatheringPolicy.CollectAllAsync()
    {
        return Task.FromResult(false);
    }
}