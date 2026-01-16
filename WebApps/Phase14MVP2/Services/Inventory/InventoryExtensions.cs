namespace Phase14MVP2.Services.Inventory;
public static class InventoryExtensions
{
    extension(InventoryManager inventory)
    {
        public int GetInventoryCount(string item) => inventory.Get(item);
    }
}