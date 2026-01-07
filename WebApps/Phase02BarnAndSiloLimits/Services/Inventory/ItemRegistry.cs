namespace Phase02BarnAndSiloLimits.Services.Inventory;
public class ItemRegistry
{
    //each player will have their own registry.

    private Dictionary<string, ItemDefinition> _items = [];

    public void Register(ItemDefinition item)
    {
        _items[item.ItemName] = item;
    }

    public ItemDefinition Get(string name) => _items.TryGetValue(name, out var def)
            ? def
            : throw new KeyNotFoundException($"Item not registered: '{name}'");

}
