namespace Phase10ProgressionUnlocks.Quests;
public interface IQuestRecipes
{
    Task<BasicList<QuestRecipe>> GetQuestsAsync();
}