namespace Phase09LevelProgression.Services.Progression;
public class ProgressionServicesContext
{
    required public ILevelProgressionPlanProvider LevelProgressionPlanProvider { get; init; }

    required public IProgressionProfile ProgressionProfile { get; init; }
    //all other services goes here.

}