using Phase08UpgradeWorkshopCapacity.Services.Core;

namespace Phase08UpgradeWorkshopCapacity.DataAccess.Quests;
public class QuestDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<QuestRecipe> Quests { get; set; }
}