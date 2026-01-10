namespace Phase08UpgradeWorkshopCapacity.Components.Custom;
public partial class TreesComponent
{
    private BasicList<TreeView> _trees = [];
    protected override void OnInitialized()
    {

        Refresh();

        base.OnInitialized();

    }
    protected override Task OnTickAsync()
    {
        Refresh();
        return base.OnTickAsync();
    }
    private void Refresh()
    {
        _trees = TreeManager.GetUnlockedTrees;
    }
   
}