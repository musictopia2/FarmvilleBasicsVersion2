namespace Phase01IntroduceBarnAndSilo.Components.Custom;
public abstract class InventoryAwareComponentBase : FarmComponentBase, IDisposable
{
    protected override void OnInitialized()
    {
        Inventory.InventoryChanged += async () => await OnInventoryChangedAsync();
        base.OnInitialized();
    }
    protected virtual async Task OnInventoryChangedAsync()
    {
        await InvokeAsync(StateHasChanged);
    }
    protected virtual void DisposeCore()
    {

    }
    public void Dispose()
    {
        Inventory?.InventoryChanged -= async () => await OnInventoryChangedAsync();
        DisposeCore();
        GC.SuppressFinalize(this);
    }
}