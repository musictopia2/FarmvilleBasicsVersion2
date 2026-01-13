namespace Phase12StoreWithBasicPurchases.Quests;
public class QuestServicesContext
{
    required public IQuestRecipes QuestRecipes { get; init; }
    required public IQuestPersistence QuestPersistence { get; set; }
}