namespace Phase10ProgressionUnlocks.Services.Progression;
public class ProgressionServicesContext
{
    required public ILevelProgressionPlanProvider LevelProgressionPlanProvider { get; init; } //used so when i level up, decide what i can now get.
    required public ICropProgressionPlanProvider CropProgressionPlanProvider { get; init; }
    required public IAnimalProgressionPlanProvider AnimalProgressionPlanProvider { get; init;  }
    required public ITreeProgressionPlanProvider TreeProgressionPlanProvider { get; init; }
    required public IWorksiteProgressionPlanProvider WorksiteProgressionPlanProvider { get; init; }
    required public IProgressionProfile ProgressionProfile { get; init; }
    //all other services goes here.

}