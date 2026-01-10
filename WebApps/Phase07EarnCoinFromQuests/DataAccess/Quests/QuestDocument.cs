using Phase07EarnCoinFromQuests.Services.Core;

namespace Phase07EarnCoinFromQuests.DataAccess.Quests;
public class QuestDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<QuestRecipe> Quests { get; set; }
}