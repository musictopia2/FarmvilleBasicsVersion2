namespace Phase06IncreaseBarnAndSiloLimits.Components.Custom;

public partial class QuestBookComponent
{
    private BasicList<QuestRecipe> _incompleteQuests = [];

    private void LoadQuests()
        => _incompleteQuests = Farm!.QuestManager.GetAllIncompleteQuests();

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

    private async Task ToggleTrackedAsync(QuestRecipe quest)
    {
        await Farm!.QuestManager.SetTrackedAsync(quest, !quest.Tracked, 4);
        LoadQuests(); // refresh star states and any ordering you choose
    }

    private async Task CompleteQuestAsync(QuestRecipe quest)
    {
        if (Farm!.QuestManager.CanCompleteQuest(quest) == false)
        {
            return;
        }

        await Farm!.QuestManager.CompleteQuestAsync(quest);
        LoadQuests();
    }

    private int InventoryAmount(string itemKey) => Inventory.Get(itemKey);
    private string GetItemImageSrc(string itemKey) => $"/{itemKey}.png";
}