namespace Phase09LevelProgression.Models;
public class QuestDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<QuestRecipe> Quests { get; set; }
}