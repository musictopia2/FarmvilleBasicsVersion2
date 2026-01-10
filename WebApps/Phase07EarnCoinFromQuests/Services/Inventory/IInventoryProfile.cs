namespace Phase07EarnCoinFromQuests.Services.Inventory;
public interface IInventoryProfile
{
    Task<InventoryStorageProfileModel> LoadAsync();
    Task SaveAsync(InventoryStorageProfileModel profile);
}