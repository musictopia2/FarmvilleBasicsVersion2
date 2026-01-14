namespace Phase13QuestsBasedOnLevel.Components.Custom;
public partial class TreeComponent(IToast toast)
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
    private string GetFruitImage => $"/{Tree.ItemName}.png";
    protected override Task OnTickAsync()
    {
        _ready = TreeManager.TreesReady(Tree);
        return base.OnTickAsync();
    }
    private void Process()
    {
        if (_ready > 0)
        {
            if (TreeManager.CanCollectFromTree(Tree) == false)
            {
                toast.ShowUserErrorToast("Unable to collect from trees because not enough room in your silo.  Try discarding some items, craft something, or fulfill orders");
                return;
            }
            TreeManager.CollectFromTree(Tree);
            _ready = TreeManager.TreesReady(Tree);
            return;
        }
        _showpopup = true;
    }
}