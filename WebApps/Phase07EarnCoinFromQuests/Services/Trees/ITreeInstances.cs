namespace Phase07EarnCoinFromQuests.Services.Trees;
public interface ITreeInstances
{
    Task<BasicList<TreeAutoResumeModel>> GetTreeInstancesAsync();
}