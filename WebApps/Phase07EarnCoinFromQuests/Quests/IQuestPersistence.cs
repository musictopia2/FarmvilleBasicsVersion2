namespace Phase07EarnCoinFromQuests.Quests;
public interface IQuestPersistence
{
    Task SaveQuestsAsync(BasicList<QuestRecipe> quests);
}