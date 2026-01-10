namespace Phase07EarnCoinFromQuests.Services.Trees;
public interface ITreePersistence
{
    Task SaveTreesAsync(BasicList<TreeAutoResumeModel> trees);
}