namespace Phase10ProgressionUnlocks.Services.Crops;
public interface ICropInstances
{
    Task<CropSystemState> GetCropInstancesAsync();
}