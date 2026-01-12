namespace Phase09LevelProgression.Services.Workshops;
public class WorkshopAutoResumeModel
{
    public Guid Id { get; set; } = Guid.NewGuid();        // <- persistent GUID

    public BasicList<string> UnlockedItems { get; set; } = [];

    public int SelectedRecipeIndex { get; set; } = 0;
    public string Name { get; set; } = "";
    public bool Unlocked { get; set; } = true;
    public int Capacity { get; set; } = 2;
    public BasicList<CraftingAutoResumeModel> Queue { get; set; } = [];
}