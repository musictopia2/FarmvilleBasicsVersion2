namespace Phase11ProgressionVisibility.Services.Progression;
public interface IWorksiteProgressionPlanProvider
{
    Task<BasicList<ItemUnlockRule>> GetPlanAsync(FarmKey farm);
}