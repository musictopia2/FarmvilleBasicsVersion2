using Phase13QuestsBasedOnLevel.RandomQuestGeneratorProcesses; //not common enough.

namespace Phase13QuestsBasedOnLevel.DataAccess.Quests;
public class QuestFactory : IQuestFactory
{
    QuestServicesContext IQuestFactory.GetQuestServices(FarmKey farm)
    {
        return new()
        {
            QuestProfile = new QuestProfileDatabase(farm),
            QuestGenerationService = new RandomQuestGenerationService() //this is somewhat simple but okay for now.  later can do more balancing things.

        };
    }
}