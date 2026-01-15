namespace Phase13QuestsBasedOnLevel.DataAccess.Quests;
public class QuestFactory : IQuestFactory
{
    QuestServicesContext IQuestFactory.GetQuestServices(FarmKey farm)
    {
        return new()
        {
            QuestProfile = new QuestProfileDatabase(farm),
            QuestGenerationService = new TemporaryQuestGeneratorClass()

        };
    }
}