namespace Phase07EarnCoinFromQuests.Services.Inventory;
//decided to go with factory so one profile can use different implementation than another.
public interface IInventoryFactory
{
    IInventoryRepository GetInventoryServices(FarmKey farm);
    IInventoryProfile GetInventoryProfile(FarmKey farm);
}