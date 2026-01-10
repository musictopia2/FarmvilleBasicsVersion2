namespace Phase06IncreaseBarnAndSiloLimits.Components.Custom;
public partial class AnimalChoiceModal(OverlayService overlay)
{
    [Parameter]
    [EditorRequired]
    public AnimalView Animal { get; set; } = default!;

    [Parameter]
    public EventCallback<int> OnOptionChosen { get; set; }

    private BasicList<AnimalProductionOption> _productionOptions = [];

    


    private async Task ClickedCropAsync(string name)
    {
        if (name == "")
        {
            return;
        }
        //just clicking means needs to open the crops.
        await overlay.OpenCropsAsync();

    }

    protected override void OnParametersSet()
    {
        _productionOptions = AnimalManager.GetUnlockedProductionOptions(Animal);
    }

    private void OptionSelected(AnimalProductionOption option)
    {
        int index = _productionOptions.IndexOf(option);
        OnOptionChosen.InvokeAsync(index);
    }

    // --- Image path helper (edit to match your project) ---
    private static string Normalize(string text)
        => text.Replace(" ", "").Replace("-", "").Replace("_", "").ToLowerInvariant();

    private static string GetItemImage(string itemName)
        => $"/{Normalize(itemName)}.png";


    private string GetDurationText(AnimalProductionOption option)
    {
        
        int index = _productionOptions.IndexOf(option);
        return AnimalManager.Duration(Animal, index); // <-- rename to your real method
    }

    private int Have(string itemKey) => InventoryManager.Get(itemKey);

    private static string Details(int has, int required) => $"{has}/{required}";


}