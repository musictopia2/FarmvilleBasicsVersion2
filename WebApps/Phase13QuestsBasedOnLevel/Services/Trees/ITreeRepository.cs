namespace Phase13QuestsBasedOnLevel.Services.Trees;
public interface ITreeRepository
{
    Task<BasicList<TreeAutoResumeModel>> LoadAsync();
    Task SaveAsync(BasicList<TreeAutoResumeModel> list);
}