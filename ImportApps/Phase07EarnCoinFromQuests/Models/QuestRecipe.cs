namespace Phase07EarnCoinFromQuests.Models;
public class QuestRecipe
{
    public string Item { get; set; } = "";
    public int Required { get; set; } //once you complete, then removes from inventory.
    public bool Completed { get; set; }
    
}