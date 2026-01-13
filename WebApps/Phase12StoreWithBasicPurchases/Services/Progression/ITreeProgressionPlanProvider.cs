namespace Phase12StoreWithBasicPurchases.Services.Progression;
public interface ITreeProgressionPlanProvider
{
    Task<BasicList<ItemUnlockRule>> GetPlanAsync(FarmKey farm);
}