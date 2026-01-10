namespace Phase08UpgradeWorkshopCapacity.Services.Crops;
public interface ICropInstances
{
    Task<CropSystemState> GetCropInstancesAsync();
}