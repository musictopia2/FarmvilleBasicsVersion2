using Phase02BarnAndSiloLimits.Services.Core;

namespace Phase02BarnAndSiloLimits.Components.Custom;
public partial class Start(NavigationManager nav, IStartFarmRegistry starts)
{
    private void NavigateTo(string farmTheme, string person, string mode)
    {
        nav.NavigateTo($"/farm/{farmTheme}/{person}/{mode}");
    }

    private BasicList<FarmKey> _farms = [];
    protected override async Task OnInitializedAsync()
    {
        _farms = await starts.GetFarmsAsync();
    }

}