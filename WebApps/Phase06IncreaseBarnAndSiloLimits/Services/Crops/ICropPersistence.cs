namespace Phase06IncreaseBarnAndSiloLimits.Services.Crops;
public interface ICropPersistence
{
    Task SaveCropsAsync(BasicList<CropAutoResumeModel> slots);
}