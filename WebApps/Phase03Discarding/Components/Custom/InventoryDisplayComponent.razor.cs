namespace Phase03Discarding.Components.Custom;
public partial class InventoryDisplayComponent(IToast toast) : InventoryAwareComponentBase
{
    [Parameter]
    [EditorRequired]
    public EnumInventoryStorageCategory InventoryStorageCategory { get; set; }

    private BasicList<ItemAmount> _list = [];
    private string _errorMessage = "";
    private string GetStatus
    {
        get
        {
            int limit;
            if (InventoryStorageCategory == EnumInventoryStorageCategory.Barn)
            {
                limit = Inventory.BarnSize;
            }
            else if (InventoryStorageCategory == EnumInventoryStorageCategory.Silo)
            {
                limit = Inventory.SiloSize;
            }
            else
            {
                limit = 0;
            }
            return $"{_list.Sum(x => x.Amount)}/{limit}"; //at least something.
        }
    }

    protected override void OnInitialized()
    {
        PopulateList();
        base.OnInitialized();
    }
    private void PopulateList()
    {
        if (InventoryStorageCategory == EnumInventoryStorageCategory.Barn)
        {
            _list = Inventory.GetAllBarnInventoryItems();
        }
        else if (InventoryStorageCategory == EnumInventoryStorageCategory.Silo)
        {
            _list = Inventory.GetAllSiloInventoryItems();
        }
        else
        {
            _errorMessage = "Can only show barn or silo items";
        }
    }
    private void DisplayInventoryItem(string itemName)
    {
        toast.ShowInfoToast(itemName.GetWords);
    }
    protected override async Task OnInventoryChangedAsync()
    {
        await base.OnInventoryChangedAsync();
        PopulateList();
    }
    // itemName is the image file name in wwwroot (root folder).
    // If itemName already includes an extension (e.g. "barn.png"), it will use it as-is.
    // If it doesn't, it assumes ".png".
    private static string GetItemImageSrc(string itemName)
    {
        return $"{itemName}.png";
    }
}