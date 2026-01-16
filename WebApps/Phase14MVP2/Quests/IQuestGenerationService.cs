namespace Phase14MVP2.Quests;

public interface IQuestGenerationService
{

    QuestInstanceModel CreateQuest(int currentLevel,
        BasicList<ItemPlanModel> eligibleItems,
        BasicList<QuestInstanceModel> existingBoard);

}
