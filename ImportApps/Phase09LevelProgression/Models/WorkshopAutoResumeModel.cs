namespace Phase09LevelProgression.Models;
public class WorkshopAutoResumeModel
{
    public Guid Id { get; set; } = Guid.NewGuid();        // <- persistent GUID
    public int SelectedRecipeIndex { get; set; } = 0;
    public string Name { get; set; } = "";
    public bool Unlocked { get; set; } = true;
    public int Capacity { get; set; } = 0; //this time, start with 0.  let it calculate.
    public BasicList<CraftingAutoResumeModel> Queue { get; set; } = [];
    public double? RunMultiplier { get; set; }
}