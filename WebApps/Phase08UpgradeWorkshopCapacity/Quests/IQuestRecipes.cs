namespace Phase08UpgradeWorkshopCapacity.Quests;
public interface IQuestRecipes
{
    Task<BasicList<QuestRecipe>> GetQuestsAsync();
}