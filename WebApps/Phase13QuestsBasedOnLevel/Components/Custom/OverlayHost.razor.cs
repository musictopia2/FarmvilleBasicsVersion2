namespace Phase13QuestsBasedOnLevel.Components.Custom;

public partial class OverlayHost(OverlayService overlay) : IDisposable
{

    

    public void Dispose()
    {
        overlay.Changed -= Refresh;
        GC.SuppressFinalize(this);
    }
    private void Refresh()
    {
        InvokeAsync(StateHasChanged);
    }
    protected override void OnInitialized()
    {
        overlay.Changed += Refresh;
        base.OnInitialized();
    }



}