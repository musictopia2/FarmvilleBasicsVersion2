namespace Phase01IntroduceBarnAndSilo.Services.Crops;
public interface ICropInstances
{
    Task<CropSystemState> GetCropInstancesAsync();
}