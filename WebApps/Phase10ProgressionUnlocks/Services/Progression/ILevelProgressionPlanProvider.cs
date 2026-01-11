namespace Phase10ProgressionUnlocks.Services.Progression;
public interface ILevelProgressionPlanProvider
{
    Task<LevelProgressionPlanModel> GetPlanAsync(FarmKey farm);
}