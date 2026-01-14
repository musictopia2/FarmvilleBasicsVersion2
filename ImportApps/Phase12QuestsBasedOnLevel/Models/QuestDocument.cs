namespace Phase12QuestsBasedOnLevel.Models;
public class QuestDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<QuestRecipe> Quests { get; set; }
}