namespace Phase04EnforcingLimits.Components.Custom;
public class OverlayService(PopupRegistry popup, FarmContext farm)
{
    public event Action? Changed;
    public IToast? Toast { get; set; }
    public bool ShowQuestBook { get; private set; }
    public bool ShowAnimals { get; private set; }
    public bool ShowTrees { get; private set; }
    public bool ShowCrops { get; private set;  }
    public bool ShowWorkshops => CurrentWorkshop is not null;
    public bool ShowWorksites => CurrentWorksite is not null;
    public async Task OpenQuestBookAsync()
    {
        await CloseAllAsync();
        ShowQuestBook = true;
        Changed?.Invoke();
    }
    public void CloseQuestBook()
    {
        ShowQuestBook = false;
        Changed?.Invoke();
    }
    public void SetWorkshopVisible(bool visible)
    {
        if (visible == false)
        {
            CurrentWorkshop = null; //i think.
            Changed?.Invoke();
        }
        
            
    }
    public void SetWorksiteVisible(bool visible)
    {
        if (visible == false)
        {
            CurrentWorksite = null;
            Changed?.Invoke();
        }
    }
    public string? CurrentWorksite { get; private set; }
    public WorkshopView? CurrentWorkshop { get; private set; }
    public async Task OpenWorkshopAsync(WorkshopView workshop)
    {
        await CloseAllAsync();
        CurrentWorkshop = workshop;
        Changed?.Invoke(); //needs this.  otherwise, won't load.
    }
    public async Task OpenPossibleWorksiteAsync(string? location)
    {
        if (string.IsNullOrWhiteSpace(location))
        {
            return;
        }
        await CloseAllAsync();
        CurrentWorksite = location;
        Changed?.Invoke();
    }
    
    public async Task OpenAnimalsAsync()
    {
        await CloseAllAsync();
        ShowAnimals = true;
        Changed?.Invoke();
    }
    public async Task OpenCropsAsync()
    {
        await CloseAllAsync();
        ShowCrops = true;
        Changed?.Invoke();
    }
    public async Task OpenTreesAsync()
    {
        await CloseAllAsync();
        ShowTrees = true;
        Changed?.Invoke();
    }
    public void SetCropsVisible(bool visible)
    {
        ShowCrops = visible;
        Changed?.Invoke();
    }
    public void SetTreesVisible(bool visible)
    {
        ShowTrees = visible;
        Changed?.Invoke();
    }
    public void SetAnimalsVisible(bool visible)
    {
        ShowAnimals = visible;
        Changed?.Invoke();
    }
    public void Reset()
    {
        ShowQuestBook = false;
        ShowTrees = false;
        ShowCrops = false;
        CurrentWorksite = null;
        ShowAnimals = false;
        CurrentWorkshop = null;
    }
    

    public async Task CloseAllAsync()
    {
        if (farm.CanCloseWorksiteAutomatically(CurrentWorksite) == false)
        {
            Toast?.ShowUserErrorToast("Must collect from worksite first before closing all popups");
            return;
        }
        Reset();
        
        await popup.CloseAllAsync(); //just in case.
        Changed?.Invoke();
    }
}