namespace Phase03Discarding.Components.Custom;
public partial class QuestTrackerComponent(OverlayService questService, IToast toast)
{

    private BasicList<QuestRecipe> _incompleteQuests = [];

    

    private const string _worksiteLockMessage = "Must collect from current worksite first";

    protected override Task OnInitializedAsync()
    {
        questService.Changed += Refresh;
        return base.OnInitializedAsync();
    }
    protected override void DisposeCore()
    {
        questService.Changed -= Refresh;
        base.DisposeCore();
    }
    private void Refresh()
    {
        LoadQuests();
        InvokeAsync(StateHasChanged);
    }

    private bool BlockIfLocked()
    {
        if (this.CanCloseWorksiteAutomatically(questService.CurrentWorksite))
        {
            return false;
        }
        toast.ShowUserErrorToast(_worksiteLockMessage);
        return true;

    }
    private async Task ShowAllQuestsAsync()
    {
        if (BlockIfLocked())
        {
            return;
        }
        await questService.OpenQuestBookAsync();
    }

    

    private void LoadQuests()
    {
        _incompleteQuests = Farm!.QuestManager.ShowCurrentQuests(4);
    }
    protected override void OnInitialized()
    {
        LoadQuests();
        base.OnInitialized();
    }
    protected override Task OnInventoryChangedAsync()
    {
        LoadQuests();
        return base.OnInventoryChangedAsync();
    }
    private bool CanCompleteQuest(QuestRecipe recipe) => Farm!.QuestManager.CanCompleteQuest(recipe);


    private async Task AttemptNavigationAsync(QuestRecipe recipe)
    {

        WorkshopView? workshop = WorkshopManager.SearchForWorkshop(recipe.Item);

        if (workshop is not null)
        {
            await questService.OpenWorkshopAsync(workshop);
            return;
        }

        //attempt animals.
        if (AnimalManager.HasAnimal(recipe.Item))
        {
            await questService.OpenAnimalsAsync();
            return;
        }
        if (CropManager.HasCrop(recipe.Item))
        {
            await questService.OpenCropsAsync();
            return;
        }
        if (TreeManager.HasTrees(recipe.Item))
        {
            await questService.OpenTreesAsync();
            return;
        }
        await questService.OpenPossibleWorksiteAsync(WorksiteManager.GetPossibleWorksiteForItem(recipe.Item));
    }
    private async Task CompleteQuestAsync(QuestRecipe recipe)
    {
        if (BlockIfLocked())
        {
            return;
        }
        if (CanCompleteQuest(recipe) == false)
        {
            await AttemptNavigationAsync(recipe);
            return;
        }
        await Farm!.QuestManager.CompleteQuestAsync(recipe);
        LoadQuests();
    }
    
    private int InventoryAmount(string itemKey)
    {
        // Whatever your inventory lookup is.
        // Example placeholder:
        return Inventory.Get(itemKey);
    }


}