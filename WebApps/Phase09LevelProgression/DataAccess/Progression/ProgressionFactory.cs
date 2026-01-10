namespace Phase09LevelProgression.DataAccess.Progression;
public class ProgressionFactory : IProgressionFactory
{
    ProgressionServicesContext IProgressionFactory.GetProgressionServices(FarmKey farm)
    {
        return new()
        {
            LevelProgressionPlanProvider = new LevelProgressionPlanDatabase(),
            ProgressionProfile = new ProgressionProfileDatabase(farm)
        };
    }
}