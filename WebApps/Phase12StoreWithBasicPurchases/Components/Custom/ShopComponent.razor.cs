namespace Phase12StoreWithBasicPurchases.Components.Custom;
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
        Console.WriteLine("Trying to purchase");
        // TODO: StoreManager.TryPurchaseAsync(row) etc.
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

    private static string CostInfo(StoreItemRowModel row)
    {
        if (row.IsMaxedOut)
        {
            return "Maxed";
        }
        if (row.IsLocked)
        {
            return $"Locked Level Required {row.LevelRequired}";
        }
        if (row.IsFree)
        {
            return "Free";
        }
        if (row.Costs.Count > 1)
        {
            return "Rethink";
        }
        return $"{row.Costs.Single().Key} {row.Costs.Single().Value}";
    }
    protected override void OnInitialized()
    {
        Reload();
        base.OnInitialized();
    }
}