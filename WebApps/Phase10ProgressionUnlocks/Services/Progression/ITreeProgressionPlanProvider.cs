namespace Phase10ProgressionUnlocks.Services.Progression;
public interface ITreeProgressionPlanProvider
{
    Task<BasicList<ItemUnlockRule>> GetPlanAsync(FarmKey farm);
}