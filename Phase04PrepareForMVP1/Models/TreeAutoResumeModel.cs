namespace Phase04PrepareForMVP1.Models;
public class TreeAutoResumeModel
{
    required public string TreeName { get; set; } = "";
    public bool Unlocked { get; set; } = true;
    public int TreesReady { get; set; } = 4; //i think.
    public EnumTreeState State { get; set; } = EnumTreeState.Collecting;
    public DateTime? StartedAt { get; set; }
    public DateTime? TempStart { get; set; }
    public double? RunMultiplier { get; set; }
    // Production timing is determined by the associated TreeRecipe.
}