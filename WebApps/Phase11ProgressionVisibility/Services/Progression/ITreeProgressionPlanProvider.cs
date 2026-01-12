namespace Phase11ProgressionVisibility.Services.Progression;
public interface ITreeProgressionPlanProvider
{
    Task<BasicList<ItemUnlockRule>> GetPlanAsync(FarmKey farm);
}