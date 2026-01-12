namespace Phase11ProgressionVisibility.Services.Trees;
public class TreeAutoResumeModel
{
    public string TreeName { get; set; } = "";
    public bool Unlocked { get; set; } = true;
    public int TreesReady { get; set; }
    public EnumTreeState State { get; set; } = EnumTreeState.Collecting;
    public DateTime? StartedAt { get; set; }
    public DateTime? TempStart { get; set; }
    public double? RunMultiplier { get; set; }
    //good news is did not save time here so at least no problem here.

    // Production timing is determined by the associated TreeRecipe.
}