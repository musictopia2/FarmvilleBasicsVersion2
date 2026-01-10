using Phase09LevelProgression.Services.Core;

namespace Phase09LevelProgression.Quests;
public interface IQuestFactory
{
    QuestServicesContext GetQuestServices(FarmKey farm);
}