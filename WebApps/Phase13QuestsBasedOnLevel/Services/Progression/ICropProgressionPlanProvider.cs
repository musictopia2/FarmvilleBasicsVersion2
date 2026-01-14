namespace Phase13QuestsBasedOnLevel.Services.Progression;
public interface ICropProgressionPlanProvider
{
    Task<CropProgressionPlanModel> GetPlanAsync(FarmKey farm);
}