namespace Phase13QuestsBasedOnLevel.DataAccess.Progression;
public class ProgressionFactory : IProgressionFactory
{
    ProgressionServicesContext IProgressionFactory.GetProgressionServices(FarmKey farm)
    {
        return new()
        {
            LevelProgressionPlanProvider = new LevelProgressionPlanDatabase(),
            ProgressionProfile = new ProgressionProfileDatabase(farm),
            CropProgressionPlanProvider = new CropProgressionPlanDatabase(),
            AnimalProgressionPlanProvider = new AnimalProgressionPlanDatabase(),
            WorkshopProgressionPlanProvider = new WorkshopProgressionPlanDatabase(),
        };
    }
}