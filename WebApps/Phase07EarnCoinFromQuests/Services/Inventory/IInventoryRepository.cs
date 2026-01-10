namespace Phase07EarnCoinFromQuests.Services.Inventory;
public interface IInventoryRepository
{
    Task SaveAsync(FarmKey farm, Dictionary<string, int> items);
    Task<Dictionary<string, int>> LoadAsync(FarmKey farm);
}