namespace Phase10ProgressionUnlocks.Services.Progression;
public interface IAnimalProgressionPlanProvider
{
    Task<BasicList<ItemUnlockRule>> GetPlanAsync(FarmKey farm);
}