namespace Phase02BarnAndSiloLimits.Services.Inventory;
public interface IInventoryProfile
{
    Task<InventoryStorageProfileModel> LoadAsync(FarmKey farm);
    Task SaveAsync(FarmKey farm, InventoryStorageProfileModel profile);
}