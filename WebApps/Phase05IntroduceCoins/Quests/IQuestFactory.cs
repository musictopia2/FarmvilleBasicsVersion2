using Phase05IntroduceCoins.Services.Core;

namespace Phase05IntroduceCoins.Quests;
public interface IQuestFactory
{
    QuestServicesContext GetQuestServices(FarmKey farm);
}