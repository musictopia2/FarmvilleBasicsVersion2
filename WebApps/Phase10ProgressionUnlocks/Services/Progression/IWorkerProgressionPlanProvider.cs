namespace Phase10ProgressionUnlocks.Services.Progression;
public interface IWorkerProgressionPlanProvider
{
    Task<BasicList<ItemUnlockRule>> GetPlanAsync(FarmKey farm);
}