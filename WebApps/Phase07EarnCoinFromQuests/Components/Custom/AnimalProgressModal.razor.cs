namespace Phase07EarnCoinFromQuests.Components.Custom;
public partial class AnimalProgressModal
{
    [Parameter]
    [EditorRequired]
    public AnimalView Animal { get; set; }
    [Parameter]
    [EditorRequired]
    public AnimalProductionOption Option { get; set; }

    private string ReadyText => $"Ready In {AnimalManager.TimeLeftForResult(Animal)}";
    private string ProducingText => $"Producing {AnimalManager.InProgress(Animal)} {Option.Output.Item.GetWords}";
    private int Have(string itemKey) => InventoryManager.Get(itemKey);
}