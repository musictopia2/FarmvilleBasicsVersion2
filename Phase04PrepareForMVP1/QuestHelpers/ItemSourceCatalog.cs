namespace Phase04PrepareForMVP1.QuestHelpers;
public sealed class ItemSourceCatalog
{
    public BasicList<ItemSourceProfile> Items { get; } = [];
    public ItemSourceProfile Get(string itemName)
    {
        var found = Items.FirstOrDefault(x => x.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        if (found != null)
        {
            return found;
        }
        throw new CustomBasicException($"{itemName} was not found");
    }

    public ItemSourceProfile GetOrCreate(string itemName, TimeSpan time, int amount)
    {
        var found = Items.FirstOrDefault(x => x.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        if (found != null)
        {
            return found;
        }
        var created = new ItemSourceProfile(itemName, time, amount);
        Items.Add(created);
        return created;
    }
}