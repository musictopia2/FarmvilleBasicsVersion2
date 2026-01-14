using Phase13QuestsBasedOnLevel.Services.Core;

namespace Phase13QuestsBasedOnLevel.Quests;
public interface IQuestFactory
{
    QuestServicesContext GetQuestServices(FarmKey farm);
}