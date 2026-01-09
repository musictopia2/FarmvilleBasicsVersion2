using Phase05IntroduceCoins.Services.Core;

namespace Phase05IntroduceCoins.DataAccess.Quests;
public class QuestDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<QuestRecipe> Quests { get; set; }
}