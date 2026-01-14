namespace Phase13QuestsBasedOnLevel.Quests;
public interface IQuestRecipes
{
    Task<BasicList<QuestRecipe>> GetQuestsAsync();
}