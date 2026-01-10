namespace Phase07EarnCoinFromQuests.Components.Custom;

public partial class OverlayHost(OverlayService questOverlay) : IDisposable
{

    

    public void Dispose()
    {
        questOverlay.Changed -= Refresh;
        GC.SuppressFinalize(this);
    }
    private void Refresh()
    {
        InvokeAsync(StateHasChanged);
    }
    protected override void OnInitialized()
    {
        questOverlay.Changed += Refresh;
        base.OnInitialized();
    }



}