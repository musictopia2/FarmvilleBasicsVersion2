using BasicBlazorLibrary.Components.Toasts;
using Phase10ProgressionUnlocks.Components.Custom;

namespace Phase10ProgressionUnlocks.Components.Pages;

public partial class Farm(GameRegistry registry, FarmContext context) : IDisposable
{
    [Parameter]
    public string Theme { get; set; } = string.Empty;

    [Parameter]
    public string Player { get; set; } = string.Empty;

    [Parameter]
    public string ProfileId { get; set; } = string.Empty;
    private MainFarmContainer? _farmContainer;
    protected override void OnInitialized()
    {
        BlazoredToasts.Timeout = 2; //needs to be less time.
        BlazoredToasts.TopOffset = "250px";
        FarmKey player = new()
        {
            Theme = Theme,
            PlayerName = Player,
            ProfileId = ProfileId
        };
        _farmContainer = registry.GetFarm(player);
        context.Set(_farmContainer);
        base.OnInitialized();
    }

    public void Dispose()
    {
        BlazoredToasts.TopOffset = "4rem"; //the default.
        GC.SuppressFinalize(this);
    }
}