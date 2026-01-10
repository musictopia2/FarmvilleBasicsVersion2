using Phase09LevelProgression.Services.Core;

namespace Phase09LevelProgression.DataAccess.Quests;
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