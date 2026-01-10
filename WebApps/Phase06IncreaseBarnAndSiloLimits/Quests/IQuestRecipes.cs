namespace Phase06IncreaseBarnAndSiloLimits.Quests;
public interface IQuestRecipes
{
    Task<BasicList<QuestRecipe>> GetQuestsAsync();
}