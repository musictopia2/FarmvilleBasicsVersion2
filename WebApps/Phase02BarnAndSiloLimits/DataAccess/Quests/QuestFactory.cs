using Phase02BarnAndSiloLimits.Services.Core;

namespace Phase02BarnAndSiloLimits.DataAccess.Quests;
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