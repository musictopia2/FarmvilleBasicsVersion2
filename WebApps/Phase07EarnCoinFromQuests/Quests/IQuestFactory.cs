using Phase07EarnCoinFromQuests.Services.Core;

namespace Phase07EarnCoinFromQuests.Quests;
public interface IQuestFactory
{
    QuestServicesContext GetQuestServices(FarmKey farm);
}