namespace Phase13QuestsBasedOnLevel.DataAccess.Quests;
public class TemporaryQuestGeneratorClass : IQuestGenerationService
{
    QuestInstanceModel IQuestGenerationService.CreateQuest(int currentLevel, BasicList<ItemPlanModel> eligibleItems, BasicList<QuestInstanceModel> existingBoard)
    {
        throw new CustomBasicException("Needs to think about how to create the next quest.");
    }
}