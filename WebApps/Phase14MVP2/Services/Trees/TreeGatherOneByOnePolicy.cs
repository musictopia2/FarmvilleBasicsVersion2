
namespace Phase14MVP2.Services.Trees;
public class TreeGatherOneByOnePolicy : ITreeGatheringPolicy
{
    Task<bool> ITreeGatheringPolicy.CollectAllAsync()
    {
        return Task.FromResult(false);
    }
}