using Phase10ProgressionUnlocks.Services.Core;

namespace Phase10ProgressionUnlocks.Quests;
public interface IQuestFactory
{
    QuestServicesContext GetQuestServices(FarmKey farm);
}