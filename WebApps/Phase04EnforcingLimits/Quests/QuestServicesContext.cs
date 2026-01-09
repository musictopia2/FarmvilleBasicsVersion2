namespace Phase04EnforcingLimits.Quests;
public class QuestServicesContext
{
    required public IQuestRecipes QuestRecipes { get; init; }
    required public IQuestPersistence QuestPersistence { get; set; }
}