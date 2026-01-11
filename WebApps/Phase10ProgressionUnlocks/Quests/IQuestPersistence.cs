namespace Phase10ProgressionUnlocks.Quests;
public interface IQuestPersistence
{
    Task SaveQuestsAsync(BasicList<QuestRecipe> quests);
}