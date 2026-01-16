namespace Phase14MVP2.Components.Custom;
public partial class QuestTrackerComponent(OverlayService questService, IToast toast)
{
    private BasicList<QuestInstanceModel> _incompleteQuests = [];
    private const string _worksiteLockMessage = "Must collect from current worksite first";
    private static string GetRewardImage(string key)
    {
        return $"{key}.png";
    }
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

    private static string NameStyle => "font-size:0.95rem;";

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
    private bool CanCompleteQuest(QuestInstanceModel quest) => Farm!.QuestManager.CanCompleteQuest(quest);


    private async Task AttemptNavigationAsync(QuestInstanceModel quest)
    {

        WorkshopView? workshop = WorkshopManager.SearchForWorkshop(quest.ItemName);

        if (workshop is not null)
        {
            await questService.OpenWorkshopAsync(workshop);
            return;
        }

        //attempt animals.
        if (AnimalManager.HasAnimal(quest.ItemName))
        {
            await questService.OpenAnimalsAsync();
            return;
        }
        if (CropManager.HasCrop(quest.ItemName))
        {
            await questService.OpenCropsAsync();
            return;
        }
        if (TreeManager.HasTrees(quest.ItemName))
        {
            await questService.OpenTreesAsync();
            return;
        }
        await questService.OpenPossibleWorksiteAsync(WorksiteManager.GetPossibleWorksiteForItem(quest.ItemName));
    }
    private async Task CompleteQuestAsync(QuestInstanceModel quest)
    {
        if (BlockIfLocked())
        {
            return;
        }
        if (CanCompleteQuest(quest) == false)
        {
            await AttemptNavigationAsync(quest);
            return;
        }
        if (quest.LevelRequired > ProgressionManager.CurrentLevel)
        {
            toast.ShowUserErrorToast("Must be higher level to complete the quest");
            return;
        }
        await Farm!.QuestManager.CompleteQuestAsync(quest);
        LoadQuests();
    }
    
    private int InventoryAmount(string itemKey)
    {
        // Whatever your inventory lookup is.
        // Example placeholder:
        return InventoryManager.Get(itemKey);
    }


}