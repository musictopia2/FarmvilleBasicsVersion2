namespace Phase12StoreWithBasicPurchases.Services.Progression;
public interface IWorksiteProgressionPlanProvider
{
    Task<BasicList<ItemUnlockRule>> GetPlanAsync(FarmKey farm);
}