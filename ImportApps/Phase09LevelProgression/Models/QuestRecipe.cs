namespace Phase09LevelProgression.Models;
public class QuestRecipe
{
    public string Item { get; set; } = "";
    public int Required { get; set; } //once you complete, then removes from inventory.
    public bool Completed { get; set; }
    public Dictionary<string, int> Rewards { get; set; } = [];
}