using Phase08UpgradeWorkshopCapacity.Services.Inventory;

namespace Phase08UpgradeWorkshopCapacity.Services.Worksites;
public class WorksiteAutoResumeModel
{
    public string Name { get; set; } = "";
    public bool Unlocked { get; set; } = true;
    public DateTime? StartedAt { get; set; }
    public EnumWorksiteState Status { get; set; } = EnumWorksiteState.None;
    public BasicList<WorkerRecipe> Workers { get; set; } = [];
    public BasicList<ItemAmount> Rewards { get; set; } = [];
    public double? RunMultiplier { get; set; }
    public bool Focused { get; set; }
    public Dictionary<string, int> FailureHistory { get; set; } = [];
}