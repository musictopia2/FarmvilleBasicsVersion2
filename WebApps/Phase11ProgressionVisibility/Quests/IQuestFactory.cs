using Phase11ProgressionVisibility.Services.Core;

namespace Phase11ProgressionVisibility.Quests;
public interface IQuestFactory
{
    QuestServicesContext GetQuestServices(FarmKey farm);
}