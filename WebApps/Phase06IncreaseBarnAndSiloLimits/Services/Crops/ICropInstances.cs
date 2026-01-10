namespace Phase06IncreaseBarnAndSiloLimits.Services.Crops;
public interface ICropInstances
{
    Task<CropSystemState> GetCropInstancesAsync();
}