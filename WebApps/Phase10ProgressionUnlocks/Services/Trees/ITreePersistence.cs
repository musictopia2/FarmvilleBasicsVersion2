namespace Phase10ProgressionUnlocks.Services.Trees;
public interface ITreePersistence
{
    Task SaveTreesAsync(BasicList<TreeAutoResumeModel> trees);
}