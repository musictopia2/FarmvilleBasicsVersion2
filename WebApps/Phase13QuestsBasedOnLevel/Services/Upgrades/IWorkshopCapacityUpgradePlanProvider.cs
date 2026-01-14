namespace Phase13QuestsBasedOnLevel.Services.Upgrades;
public interface IWorkshopCapacityUpgradePlanProvider
{
    Task<BasicList<WorkshopCapacityUpgradePlanModel>> GetPlansAsync(FarmKey farm);
}