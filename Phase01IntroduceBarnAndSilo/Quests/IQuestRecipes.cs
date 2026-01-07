namespace Phase01IntroduceBarnAndSilo.Quests;
public interface IQuestRecipes
{
    Task<BasicList<QuestRecipe>> GetQuestsAsync();
}