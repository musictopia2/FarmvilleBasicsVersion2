namespace Phase01IntroduceBarnAndSilo.Quests;
public interface IQuestPersistence
{
    Task SaveQuestsAsync(BasicList<QuestRecipe> quests);
}