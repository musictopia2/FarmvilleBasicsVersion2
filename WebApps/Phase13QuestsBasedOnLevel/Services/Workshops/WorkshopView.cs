namespace Phase13QuestsBasedOnLevel.Services.Workshops;
public class WorkshopView
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public int SelectedRecipeIndex { get; set; }
    public int ReadyCount { get; set; }
}
