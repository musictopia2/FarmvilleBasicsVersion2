using Phase04EnforcingLimits.Services.Core;

namespace Phase04EnforcingLimits.Quests;
public interface IQuestFactory
{
    QuestServicesContext GetQuestServices(FarmKey farm);
}