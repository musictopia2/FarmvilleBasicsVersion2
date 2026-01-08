
namespace Phase03Discarding.Services.Trees;
public class TreeGatherAllPolicy : ITreeGatheringPolicy
{
    Task<bool> ITreeGatheringPolicy.CollectAllAsync()
    {
        return Task.FromResult(true);
    }
}