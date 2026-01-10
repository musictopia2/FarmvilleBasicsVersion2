using Phase06IncreaseBarnAndSiloLimits.Services.Core;

namespace Phase06IncreaseBarnAndSiloLimits.DataAccess.Quests;
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