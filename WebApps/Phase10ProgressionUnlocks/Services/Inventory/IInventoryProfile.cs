namespace Phase10ProgressionUnlocks.Services.Inventory;
public interface IInventoryProfile
{
    Task<InventoryStorageProfileModel> LoadAsync();
    Task SaveAsync(InventoryStorageProfileModel profile);
}