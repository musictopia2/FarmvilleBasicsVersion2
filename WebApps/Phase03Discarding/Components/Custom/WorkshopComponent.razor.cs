
namespace Phase03Discarding.Components.Custom;
public partial class WorkshopComponent
{
    [Parameter]
    [EditorRequired]
    public WorkshopView Workshop { get; set; }

    private BasicList<WorkshopRecipeSummary> _recipes = [];

    [Parameter]
    public EventCallback NextClicked { get; set; }

    [Parameter]
    public EventCallback PreviousClicked { get; set; }


    [Parameter]
    public EventCallback<string> NavigateTo { get; set; }


    private int _capacity;



    protected override void OnParametersSet()
    {
        _recipes = WorkshopManager.GetRecipesForWorkshop(Workshop);
        if (_recipes.Count > 0)
        {
            Workshop.SelectedRecipeIndex = Math.Clamp(
                Workshop.SelectedRecipeIndex, 0, _recipes.Count - 1
            );
        }
       
        _capacity = WorkshopManager.GetCapcity(Workshop);
        base.OnParametersSet();
    }
    private WorkshopRecipeSummary CurrentRecipe => _recipes[Workshop.SelectedRecipeIndex];
    private string ChosenItem => CurrentRecipe.Item;
    private bool CanCraft => WorkshopManager.CanCraft(Workshop, ChosenItem);
    private void Craft()
    {
        if (CanCraft == false)
        {
            return; 
        }
        WorkshopManager.StartCraftingJob(Workshop, ChosenItem);
    }
    private int CurrentAmount => Inventory.GetInventoryCount(ChosenItem);
    private string DurationText
    {
        get
        {
            return CurrentRecipe.Duration.GetTimeString;
        }
    }
    private bool CanGoUp => Workshop.SelectedRecipeIndex > 0;
    private bool CanGoDown => Workshop.SelectedRecipeIndex < _recipes.Count - 1;

    private void GoUp()
    {
        Workshop.SelectedRecipeIndex--;
        WorkshopManager.UpdateSelectedRecipe(Workshop, Workshop.SelectedRecipeIndex);
    }

    private void GoDown()
    {
        Workshop.SelectedRecipeIndex++;
        WorkshopManager.UpdateSelectedRecipe(Workshop, Workshop.SelectedRecipeIndex);
    }
    protected override Task OnTickAsync()
    {

        if (WorkshopManager.CanPickupManually(Workshop))
        {
            WorkshopManager.PickupManually(Workshop);
        }
        return base.OnTickAsync();
    }
    
    private Dictionary<string, int> FullRequirements
    {
        get
        {
            return CurrentRecipe.Inputs;
        }
    }

    private string GetRequirementClass(string item, int required)
    {
        if (Inventory.Has(item, required))
        {
            return css1.TextSuccess;
        }
        return css1.TextDanger;
    }
    private string GetRequirementDetails(string item, int required)
    {
        int count = Inventory.GetInventoryCount(item);
        return $"{count}/{required}";
    }
    
    private static string Image(CraftingSummary craft) => $"/{craft.Name}.png";
    private static string GetItemImageSrc(string item) => $"/{item}.png";
    private static string GetTimeText(CraftingSummary craft) => craft.ReadyTime;
    private string WorkshopImage => $"/{Workshop.Name}.png";
    

}