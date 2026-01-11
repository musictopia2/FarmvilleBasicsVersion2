namespace Phase10ProgressionUnlocks.Services.Trees;
public interface ITreeInstances
{
    Task<BasicList<TreeAutoResumeModel>> GetTreeInstancesAsync();
}