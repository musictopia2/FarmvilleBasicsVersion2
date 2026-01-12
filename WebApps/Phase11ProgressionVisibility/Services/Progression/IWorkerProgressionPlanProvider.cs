namespace Phase11ProgressionVisibility.Services.Progression;
public interface IWorkerProgressionPlanProvider
{
    Task<BasicList<ItemUnlockRule>> GetPlanAsync(FarmKey farm);
}