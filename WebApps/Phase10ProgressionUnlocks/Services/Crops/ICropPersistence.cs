namespace Phase10ProgressionUnlocks.Services.Crops;
public interface ICropPersistence
{
    Task SaveCropsAsync(BasicList<CropAutoResumeModel> slots);
}