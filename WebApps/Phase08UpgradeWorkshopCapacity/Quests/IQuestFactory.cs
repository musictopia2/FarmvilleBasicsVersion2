using Phase08UpgradeWorkshopCapacity.Services.Core;

namespace Phase08UpgradeWorkshopCapacity.Quests;
public interface IQuestFactory
{
    QuestServicesContext GetQuestServices(FarmKey farm);
}