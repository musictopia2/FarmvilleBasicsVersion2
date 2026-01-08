namespace Phase03Discarding.Components.Custom;
public partial class TreeComponent
{
    [Parameter]
    [EditorRequired]
    public TreeView Tree { get; set; }

    private int _ready;
    private bool _showpopup = false;


    protected override void OnInitialized()
    {
        _ready = TreeManager.TreesReady(Tree);
        base.OnInitialized();
    }
    private string GetFruitImage => $"/{Tree.Name}.png";
    protected override Task OnTickAsync()
    {
        _ready = TreeManager.TreesReady(Tree);
        return base.OnTickAsync();
    }
    private async void ProcessAsync()
    {
        if (_ready > 0)
        {
            await TreeManager.CollectFromTreeAsync(Tree);
            _ready = TreeManager.TreesReady(Tree);
            return;
        }
        _showpopup = true;
    }
}