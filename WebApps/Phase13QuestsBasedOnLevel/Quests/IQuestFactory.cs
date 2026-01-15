namespace Phase13QuestsBasedOnLevel.Quests;
public interface IQuestFactory
{
    QuestServicesContext GetQuestServices(FarmKey farm);
}