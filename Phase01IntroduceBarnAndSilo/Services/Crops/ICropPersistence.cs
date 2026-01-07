namespace Phase01IntroduceBarnAndSilo.Services.Crops;
public interface ICropPersistence
{
    Task SaveCropsAsync(BasicList<CropAutoResumeModel> slots);
}