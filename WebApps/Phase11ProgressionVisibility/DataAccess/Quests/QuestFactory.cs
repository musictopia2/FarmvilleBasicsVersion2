using Phase11ProgressionVisibility.Services.Core;

namespace Phase11ProgressionVisibility.DataAccess.Quests;
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