using Phase03Discarding.Services.Core;

namespace Phase03Discarding.Quests;
public interface IQuestFactory
{
    QuestServicesContext GetQuestServices(FarmKey farm);
}