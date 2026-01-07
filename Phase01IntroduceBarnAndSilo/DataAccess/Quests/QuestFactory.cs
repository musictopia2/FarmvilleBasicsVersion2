using Phase01IntroduceBarnAndSilo.Services.Core;

namespace Phase01IntroduceBarnAndSilo.DataAccess.Quests;
public class QuestFactory : IQuestFactory
{
    QuestServicesContext IQuestFactory.GetQuestServices(FarmKey farm)
    {
        QuestInstanceDatabase db = new(farm);
        return new()
        {
            QuestPersistence = db,
            QuestRecipes = db,
        };
    }
}