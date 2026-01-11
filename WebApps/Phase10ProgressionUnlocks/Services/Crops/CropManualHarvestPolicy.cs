namespace Phase10ProgressionUnlocks.Services.Crops;
public class CropManualHarvestPolicy : ICropHarvestPolicy
{
    Task<bool> ICropHarvestPolicy.IsAutomaticAsync()
    {
        return Task.FromResult(false);
    }
}