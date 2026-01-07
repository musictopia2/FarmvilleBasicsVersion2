using BasicBlazorLibrary.Components.Layouts;
using BasicBlazorLibrary.Components.NavigationMenus;

namespace Phase01IntroduceBarnAndSilo.Components.Custom;
public partial class MainComponent(NavigationManager nav, OverlayService service, IToast toast) 
{

    [Parameter]
    public string Theme { get; set; } = string.Empty;

    [Parameter]
    public string Player { get; set; } = string.Empty;

    [Parameter]
    public string ProfileId { get; set; } = string.Empty;

    private NavigationBarContainer? _nav;

    private OverlayInsets _overlays = new();
    protected override void OnAfterRender(bool firstRender)
    {
        if (_nav is not null)
        {
            _overlays.TopPx = _nav.HeightOfHeader + 5;
            _overlays.BottomPx = 10;
            StateHasChanged();
            return;
        }
        base.OnAfterRender(firstRender);
    }
    protected override void OnInitialized()
    {
        service.Toast = toast;
        base.OnInitialized();
    }
    protected override async Task OnInitializedAsync()
    {
        await service.CloseAllAsync();
        await base.OnInitializedAsync();
    }
    


    private void ChooseAnotherTheme()
    {
        service.Reset();
        nav.NavigateTo("/");
    }
    private bool _showBarn = false;
    private bool _showSilo = false;

    private async Task CloseAllPopupsAsync()
    {
        await service.CloseAllAsync();
    }

    private void ShowSilo()
    {
        _showSilo = true;
    }
    
    private void ShowBarn()
    {
        _showBarn = true;
    }
    private void CloseBarn()
    {
        _showBarn = false;
    }
    
   
    private string Title
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Theme))
            {
                return "Needs Theme";
            }
            return $"{Player} {Theme.GetWords} {ProfileId.GetWords}";
        }
    }
}