namespace Phase07EarnCoinFromQuests.Quests;
public interface IQuestRecipes
{
    Task<BasicList<QuestRecipe>> GetQuestsAsync();
}