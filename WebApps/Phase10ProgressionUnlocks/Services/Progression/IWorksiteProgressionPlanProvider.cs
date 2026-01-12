namespace Phase10ProgressionUnlocks.Services.Progression;
public interface IWorksiteProgressionPlanProvider
{
    Task<BasicList<ItemUnlockRule>> GetPlanAsync(FarmKey farm);
}