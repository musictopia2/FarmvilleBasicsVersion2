namespace Phase04EnforcingLimits.Services.Inventory;
public class InventoryManager(FarmKey farm, IInventoryRepository persist,
    ItemRegistry itemRegistry
    )
{
    //does not need to send the profile here yet.
    private readonly Dictionary<string, int> _items = [];
    private InventoryStorageProfileModel _inventoryProfileModel = null!;
    public event Action? InventoryChanged;
    public void LoadStartingInventory(Dictionary<string, int> items, InventoryStorageProfileModel storage)
    {
        _inventoryProfileModel = storage;
        _items.Clear();
        foreach (var item in items)
        {
            _items.Add(item.Key, item.Value);
        }
        InventoryChanged?.Invoke(); //not sure but do just in case.
    }
    public int BarnSize => _inventoryProfileModel.BarnSize;
    public int SiloSize => _inventoryProfileModel.SiloSize;
    public int Get(string item)
    {
        return _items.GetValueOrDefault(item);
    }
    public bool Has(string item, int amount)
        => Get(item) >= amount;
    public bool Has(Dictionary<string, int> requirements)
        => requirements.All(r => Has(r.Key, r.Value));

    public bool CanAdd(ItemAmount item) => CanAdd(item.Item, item.Amount);
    public bool CanAdd(string item, int amount)
    {
        EnumInventoryStorageCategory category = itemRegistry.Get(item).Storage;
        if (category == EnumInventoryStorageCategory.None)
        {
            return true; //for now.
        }
        BasicList<ItemAmount> list;
        int limit;
        if (category == EnumInventoryStorageCategory.Barn)
        {
            //look at barn.
            list = GetAllBarnInventoryItems();
            limit = BarnSize;
        }
        else if (category == EnumInventoryStorageCategory.Silo)
        {
            list = GetAllSiloInventoryItems();
            limit = SiloSize;
        }
        else
        {
            throw new CustomBasicException("CanAdd Not Supported Yet");
        }



        int currentSize = list.Sum(x => x.Amount);
        currentSize += amount;
        if (currentSize > limit)
        {
            return false;
        }
        return true;
    }

    public void Add(string item, int amount)
    {
        if (amount <= 0)
        {
            return;
        }
        _items[item] = Get(item) + amount;
        Update();
    }
    private async void Update()
    {
        await persist.SaveAsync(farm, _items);
        InventoryChanged?.Invoke();
    }
    public void Consume(ItemAmount item)
    {
        Consume(item.Item, item.Amount);
    }
    public void Consume(string item, int amount)
    {
        if (Has(item, amount) == false)
        {
            throw new InvalidOperationException("Not enough items");
        }
        _items[item] -= amount;
        Update();
    }
    public void Consume(Dictionary<string, int> requirements)
    {
        if (Has(requirements) == false)
        {
            throw new InvalidOperationException("Not enough items");
        }
        foreach (var req in requirements)
        {
            _items[req.Key] -= req.Value;
        }
        Update();
    }

    public BasicList<ItemAmount> GetAllBarnInventoryItems()
    {
        var output = GetAllInventoryItems();
        output.KeepConditionalItems(x => HasProperItem(x.Item, EnumInventoryStorageCategory.Barn));
        return output;
    }

    private bool HasProperItem(string item, EnumInventoryStorageCategory category) => itemRegistry.Get(item).Storage == category;

    public BasicList<ItemAmount> GetAllSiloInventoryItems()
    {
        var output = GetAllInventoryItems();
        output.KeepConditionalItems(x => HasProperItem(x.Item, EnumInventoryStorageCategory.Silo));
        return output;
    }


    private BasicList<ItemAmount> GetAllInventoryItems()
    {
        return _items
            .Where(kvp => kvp.Value > 0)
            .Select(kvp => new ItemAmount(kvp.Key, kvp.Value))
            .ToBasicList();
    }
}