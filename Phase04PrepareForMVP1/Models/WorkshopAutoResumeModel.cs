namespace Phase04PrepareForMVP1.Models;
public class WorkshopAutoResumeModel
{
    public Guid Id { get; set; } = Guid.NewGuid();        // <- persistent GUID
    public int SelectedRecipeIndex { get; set; } = 0;
    public string Name { get; set; } = "";
    public bool Unlocked { get; set; } = true;
    public int Capacity { get; set; } = 5; //this time, set to 5.
    public BasicList<CraftingAutoResumeModel> Queue { get; set; } = [];
    public double? RunMultiplier { get; set; }
}