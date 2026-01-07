namespace Phase04PrepareForMVP1.Models;
public class CropRecipeDocument
{
    required public string Item { get; init; }
    public TimeSpan Duration { get; init; }
    required public string Theme { get; init; } //mode does not matter anymore.
}