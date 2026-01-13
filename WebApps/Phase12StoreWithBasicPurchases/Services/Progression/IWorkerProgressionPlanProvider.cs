namespace Phase12StoreWithBasicPurchases.Services.Progression;
public interface IWorkerProgressionPlanProvider
{
    Task<BasicList<ItemUnlockRule>> GetPlanAsync(FarmKey farm);
}