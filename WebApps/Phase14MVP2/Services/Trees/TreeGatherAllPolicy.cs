
namespace Phase14MVP2.Services.Trees;
public class TreeGatherAllPolicy : ITreeGatheringPolicy
{
    Task<bool> ITreeGatheringPolicy.CollectAllAsync()
    {
        return Task.FromResult(true);
    }
}