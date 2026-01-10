namespace Phase08UpgradeWorkshopCapacity.DataAccess.Crops;
public class BasicCropPolicy : ICropProgressionPolicy
{
    Task<bool> ICropProgressionPolicy.CanLockCropAsync(BasicList<CropDataModel> crops, string cropName)
    {
        return Task.FromResult(false);
    }

    Task<bool> ICropProgressionPolicy.CanLockGrowSlotAsync(BasicList<CropSlotState> slots)
    {
        return Task.FromResult(false);
    }

    Task<bool> ICropProgressionPolicy.CanUnlockCropAsync(BasicList<CropDataModel> crops, string cropName)
    {
        return Task.FromResult(false);
    }

    Task<bool> ICropProgressionPolicy.CanUnlockGrowSlotsAsync(BasicList<CropSlotState> slots, int slotsToUnlock)
    {
        return Task.FromResult(false);
    }

    Task ICropProgressionPolicy.LockCropAsync(BasicList<CropDataModel> crops, string cropName)
    {
        throw new NotImplementedException();
    }

    Task ICropProgressionPolicy.LockGrowSlotAsync(BasicList<CropSlotState> slots)
    {
        throw new NotImplementedException();
    }

    Task ICropProgressionPolicy.UnlockCropAsync(BasicList<CropDataModel> crops, string cropName)
    {
        throw new NotImplementedException();
    }

    Task ICropProgressionPolicy.UnlockGrowSlotsAsync(BasicList<CropSlotState> slots, int slotsToUnlock)
    {
        throw new NotImplementedException();
    }
}