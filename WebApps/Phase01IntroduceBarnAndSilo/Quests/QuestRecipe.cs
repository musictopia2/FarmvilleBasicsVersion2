namespace Phase01IntroduceBarnAndSilo.Quests;
public class QuestRecipe
{
    public string Item { get; set; } = "";
    public int Required { get; set; } //once you complete, then removes from inventory.
    public bool Completed { get; set; }
    public bool Tracked { get; set; }
    public int Order { get; set; }
}