namespace Phase14MVP2.Components.Custom;
public partial class ShopComponent(IToast toast)
{
    private async Task OnActiveKeyChanged(string? key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return;
        }

        if (Enum.TryParse<EnumCatalogCategory>(key, out var category))
        {
            await StoreManager.ChoseNewCategoryAsync(category);
            Reload();
        }
    }
    private BasicList<StoreItemRowModel> _list = [];
    private void Reload()
    {
        _list = StoreManager.GetStoreItems();
    }


    private readonly (EnumCatalogCategory Category, string Text)[] Tabs =
    [
        (EnumCatalogCategory.Tree,     "Trees"),
        (EnumCatalogCategory.Animal,   "Animals"),
        (EnumCatalogCategory.Workshop, "Workshops"),
        (EnumCatalogCategory.Worker,   "Workers"),
        (EnumCatalogCategory.Worksite, "Worksites"),
    ];

    private StoreItemRowModel? _currentItem;
    private bool _showConfirmation;
    private async Task ConfirmedAsync()
    {
        if (_currentItem is null)
        {
            return;
        }
        _showConfirmation = false;
        await StoreManager.AcquireAsync(_currentItem);
        Reload();
        toast.ShowSuccessToast($"Successfully acquired {_currentItem.TargetName.GetWords}");
        _currentItem = null;
    }

    private void OnRowClicked(StoreItemRowModel row)
    {
        // You said you need the outer div for click; keep it here.
        // Example: if locked/maxed you could ignore, else attempt purchase.
        if (row.IsLocked || row.IsMaxedOut)
        {
            return;
        }
        if (StoreManager.CanAcquire(row) == false)
        {
            toast.ShowUserErrorToast("Unable to purchase because not enough resources"); //not always coins.
            return;
        }
        _currentItem = row;
        _showConfirmation = true;
    }

    private static string CardStateClass(StoreItemRowModel row)
    {
        if (row.IsLocked)
        {
            return "is-locked";
        }

        if (row.IsMaxedOut)
        {
            return "is-maxed";
        }

        return "is-ready";
    }

    private static string CostImageUrl(string currencyKeyOrItemKey)
    {
        // Adjust to your actual currency/item icon rules.
        // Example assumes icons live at root with .png
        return $"/{currencyKeyOrItemKey}.png";
    }

    private static string ImageUrl(StoreItemRowModel row)
    {
        if (row.Category == EnumCatalogCategory.Tree)
        {
            return $"/tree.png";
        }
        return $"/{row.TargetName}.png";
    }

    protected override void OnInitialized()
    {
        Reload();
        base.OnInitialized();
    }
}