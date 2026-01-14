namespace Phase13QuestsBasedOnLevel.Quests;
public interface IQuestPersistence
{
    Task SaveQuestsAsync(BasicList<QuestRecipe> quests);
}