namespace Phase13QuestsBasedOnLevel.Quests;
public interface IQuestProfile
{
    Task<BasicList<QuestInstanceModel>> LoadAsync();
    Task SaveAsync(BasicList<QuestInstanceModel> quests);
}