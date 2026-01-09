namespace Phase04EnforcingLimits.Services.Crops;
public interface ICropProgressionPolicy
{
    // Crop types (what crops are available)
    Task<bool> CanUnlockCropAsync(BasicList<CropDataModel> crops, string cropName);
    Task UnlockCropAsync(BasicList<CropDataModel> crops, string cropName);
    Task<bool> CanLockCropAsync(BasicList<CropDataModel> crops, string cropName);
    Task LockCropAsync(BasicList<CropDataModel> crops, string cropName);

    // Grow slots (how many can be grown at once)
    Task<bool> CanUnlockGrowSlotsAsync(BasicList<CropSlotState> slots, int slotsToUnlock);
    Task UnlockGrowSlotsAsync(BasicList<CropSlotState> slots, int slotsToUnlock);
    Task<bool> CanLockGrowSlotAsync(BasicList<CropSlotState> slots);
    Task LockGrowSlotAsync(BasicList<CropSlotState> slots);
}