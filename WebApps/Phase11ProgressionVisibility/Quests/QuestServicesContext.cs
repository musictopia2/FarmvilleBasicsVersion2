namespace Phase11ProgressionVisibility.Quests;
public class QuestServicesContext
{
    required public IQuestRecipes QuestRecipes { get; init; }
    required public IQuestPersistence QuestPersistence { get; set; }
}