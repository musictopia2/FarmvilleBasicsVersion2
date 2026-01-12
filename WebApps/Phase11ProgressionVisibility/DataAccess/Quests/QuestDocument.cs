using Phase11ProgressionVisibility.Services.Core;

namespace Phase11ProgressionVisibility.DataAccess.Quests;
public class QuestDocument : IFarmDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<QuestRecipe> Quests { get; set; }
}