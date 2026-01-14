namespace Phase13QuestsBasedOnLevel.Services.Progression;
public interface IWorkshopProgressionPlanProvider
{
    Task<BasicList<ItemUnlockRule>> GetPlanAsync(FarmKey farm);
}