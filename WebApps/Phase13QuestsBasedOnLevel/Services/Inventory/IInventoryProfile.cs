namespace Phase13QuestsBasedOnLevel.Services.Inventory;
public interface IInventoryProfile
{
    Task<InventoryStorageProfileModel> LoadAsync();
    Task SaveAsync(InventoryStorageProfileModel profile);
}