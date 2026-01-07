namespace Phase04PrepareForMVP1.Models;
public class WorksiteRecipeDocument
{
    public Dictionary<string, int> Inputs { get; init; } = [];
    public string Location { get; set; } = ""; //try to put location here.
    public TimeSpan Duration { get; init; }
    public BasicList<WorksiteBaselineBenefit> BaselineBenefits { get; init; } = [];
    public int MaximumWorkers { get; set; }
    required public string StartText { get; init; }
    required public string Theme { get; init; }
    public bool CanFocus { get; set; } = true;
}