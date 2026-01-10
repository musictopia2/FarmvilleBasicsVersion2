namespace Phase07EarnCoinFromQuests.Components.Custom;
public partial class WorkshopComponent(IToast toast)
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
    private bool _showToast = true;
    protected override void OnParametersSet()
    {
        _showToast = true; //good news is when the readycount increases since something is ready from the parent calls this so i actually get desired behavior.

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
    private int CurrentAmount => InventoryManager.GetInventoryCount(ChosenItem);
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
            if (WorkshopManager.CanAddToInventory(Workshop))
            {
                WorkshopManager.PickupManually(Workshop);
                _showToast = true;
            }
            else if (_showToast)
            {
                toast.ShowUserErrorToast("Unable to pick up crafted item because the barn is full.  Try discarding or consuming the items");
                _showToast = false;
            }
        }
        else if (_showToast == false)
        {
            _showToast = true;
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
    private List<CraftingSummary> GetVisibleQueue()
    {
        var list = new List<CraftingSummary>(_capacity);

        // your slots appear to be 1-based
        for (int slot = 1; slot <= _capacity; slot++)
        {
            var s = WorkshopManager.GetSingleCraftedItem(Workshop, slot);
            if (s is null)
            {
                continue;
            }

            // Hide ready-to-pickup items completely from the queue UI
            if (s.State == EnumWorkshopState.ReadyToPickUpManually)
            {
                continue;
            }

            list.Add(s);
        }

        return list;
    }

    private static string SlotClass(int displaySlot) =>
        displaySlot == 1 ? "queue-slot-active" : "queue-slot-inactive";

    //private bool CanShowStack()

    private static string Image(CraftingSummary craft) => $"/{craft.Name}.png";
    private static string GetTimeText(CraftingSummary craft) => craft.ReadyTime;
    private string WorkshopImage => $"/{Workshop.Name}.png";


}