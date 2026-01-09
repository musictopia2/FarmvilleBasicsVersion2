namespace Phase04EnforcingLimits.Components.Custom;
public partial class WorkshopsComponent(OverlayService overlay) : IDisposable
{

    [Parameter]
    public WorkshopView? SpecificWorkshop { get; set; }

    //private bool _showAnimals;
    //private bool _showCrops;
    //private bool _showTrees;
    //private bool _showWorksites;
    //private string? _worksiteLocation;
    
    private BasicList<WorkshopView> _workshops = [];
    private WorkshopView? _currentWorkshop;
    override protected void OnInitialized()
    {
        WorkshopManager.OnWorkshopsUpdated += Refresh;
        UpdateWorkshops();
        base.OnInitialized();
    }

    private Guid? _lastSpecificId;
    private int? _lastSpecificIndex;

    protected override void OnParametersSet()
    {
        if (SpecificWorkshop is not null)
        {
            bool changed =
                _lastSpecificId != SpecificWorkshop.Id ||
                _lastSpecificIndex != SpecificWorkshop.SelectedRecipeIndex;

            if (changed)
            {
                _lastSpecificId = SpecificWorkshop.Id;
                _lastSpecificIndex = SpecificWorkshop.SelectedRecipeIndex;

                // IMPORTANT: set current workshop/selection ONCE for this request
                var existing = _workshops.SingleOrDefault(x => x.Id == SpecificWorkshop.Id);
                if (existing is not null)
                {
                    existing.SelectedRecipeIndex = SpecificWorkshop.SelectedRecipeIndex;
                    _currentWorkshop = existing;
                }
                else
                {
                    _currentWorkshop = SpecificWorkshop;
                }
            }
        }

        base.OnParametersSet();
    }
    private void UpdateWorkshops()
    {
        _workshops = WorkshopManager.GetUnlockedWorkshops;
        
    }

    private async Task TryAnimalsAsync(string name)
    {
        if (AnimalManager.HasAnimal(name))
        {
            await overlay.OpenAnimalsAsync();
            return;
        }
        await TryCropsAsync(name);
        //can eventually try crops and even trees.
    }
    
    private async Task TryCropsAsync(string name)
    {
        if (CropManager.HasCrop(name))
        {
            await overlay.OpenCropsAsync();
            return;
        }
        await TryTreesAsync(name);
    }
    private async Task TryTreesAsync(string name)
    {
        if (TreeManager.HasTrees(name))
        {
            await overlay.OpenTreesAsync();
            return;
        }
        await TryWorksitesAsync(name);
    }
    private async Task TryWorksitesAsync(string name)
    {
        
        string? location = WorksiteManager.GetPossibleWorksiteForItem(name);
        await overlay.OpenPossibleWorksiteAsync(location);
    }
    private async Task NavigateToAsync(string name)
    {
        var possible = WorkshopManager.SearchForWorkshop(name);
        if (possible is null)
        {
            await TryAnimalsAsync(name);
            return;
        }




        var existing = _workshops.Single(x => x.Id == possible.Id);

        // IMPORTANT: bring the new selected index into the object the UI is actually using
        existing.SelectedRecipeIndex = possible.SelectedRecipeIndex;

        _currentWorkshop = existing;

        await InvokeAsync(StateHasChanged);
    }

    private void Previous()
    {
        if (_workshops.Count == 0)
        {
            return;
        }

        // If nothing selected yet, pick the first (or last—either is fine; I’ll use last for "Previous")
        if (_currentWorkshop is null)
        {
            _currentWorkshop = _workshops[^1];
            return;
        }

        int index = _workshops.IndexOf(_currentWorkshop);

        // If current isn't found (list refreshed), fall back safely
        if (index < 0)
        {
            _currentWorkshop = _workshops[^1];
            return;
        }

        int prevIndex = (index - 1 + _workshops.Count) % _workshops.Count;
        _currentWorkshop = _workshops[prevIndex];
    }

    private void Next()
    {
        if (_workshops.Count == 0)
        {
            return;
        }

        // If nothing selected yet, pick the first
        if (_currentWorkshop is null)
        {
            _currentWorkshop = _workshops[0];
            return;
        }

        int index = _workshops.IndexOf(_currentWorkshop);

        // If current isn't found (list refreshed), fall back safely
        if (index < 0)
        {
            _currentWorkshop = _workshops[0];
            return;
        }

        int nextIndex = (index + 1) % _workshops.Count;
        _currentWorkshop = _workshops[nextIndex];
    }


    private void Refresh()
    {
        UpdateWorkshops();
        InvokeAsync(StateHasChanged);
    }
    

    private void SelectWorkshop(WorkshopView workshop)
    {
        if (workshop.ReadyCount > 0)
        {
            //try to collect.  if it fails, then rethink here.
            if (WorkshopManager.CanPickupManually(workshop) == false)
            {
                // TODO: show "Barn full" / discard UI when storage limits are enabled
                return;
            }
            WorkshopManager.PickupManually(workshop);
            return;
        }
        _currentWorkshop = workshop;
       
    }
    private static string Image(WorkshopView workshop) => $"/{workshop.Name}.png";

    public void Dispose()
    {
        WorkshopManager?.OnWorkshopsUpdated -= Refresh;
        GC.SuppressFinalize(this);
    }


}