namespace Phase03Discarding.Components.Custom;
public partial class InventoryDisplayComponent(IToast toast) : InventoryAwareComponentBase
{
    [Parameter]
    [EditorRequired]
    public EnumInventoryStorageCategory InventoryStorageCategory { get; set; }

    private BasicList<ItemAmount> _list = [];
    private string _errorMessage = "";

    private bool _showDiscard;
    private string _item = "";
    private int _currentSize;

    private void DiscardInventoryItem(ItemAmount item)
    {
        if (item.Item != _item)
        {
            return; //can't do anyways.
        }
        //for now, just a toast to prove everything works.
        //toast.ShowInfoToast($"So far, discarding {item.Item} with the amount of {item.Amount}");
        _showDiscard = false;

        Inventory.Consume(item);
        //hopefully is notified naturally anyways.

    }

    private void CancelDiscard()
    {
        _showDiscard = false;
        _item = "";
    }
    private int _limit;
    private string GetStatus
    {
        get
        {

            return $"Have {_currentSize} Limit {_limit}"; //at least something.
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
        //figure out how to make limit changes show up (well see).
        if (InventoryStorageCategory == EnumInventoryStorageCategory.Barn)
        {
            _limit = Inventory.BarnSize;
        }
        else if (InventoryStorageCategory == EnumInventoryStorageCategory.Silo)
        {
            _limit = Inventory.SiloSize;
        }
        else
        {
            _limit = 0;
        }
        _currentSize = _list.Sum(x => x.Amount);

    }
    private void DisplayInventoryItem(string itemName)
    {
        toast.ShowInfoToast(itemName.GetWords);
    }
    private void OpenDiscard(string name)
    {
        _item = name;
        _showDiscard = true;
        //toast.ShowSuccessToast($"Attempting to open discard for {name}");
    }
    protected override async Task OnInventoryChangedAsync()
    {
        PopulateList();
        await base.OnInventoryChangedAsync();
    }
    // itemName is the image file name in wwwroot (root folder).
    // If itemName already includes an extension (e.g. "barn.png"), it will use it as-is.
    // If it doesn't, it assumes ".png".
    private static string GetItemImageSrc(string itemName)
    {
        return $"{itemName}.png";
    }
}