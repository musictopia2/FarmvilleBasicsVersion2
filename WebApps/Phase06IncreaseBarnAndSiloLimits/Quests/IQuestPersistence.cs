namespace Phase06IncreaseBarnAndSiloLimits.Quests;
public interface IQuestPersistence
{
    Task SaveQuestsAsync(BasicList<QuestRecipe> quests);
}