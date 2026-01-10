namespace Phase07EarnCoinFromQuests.Services.Crops;
public interface ICropPersistence
{
    Task SaveCropsAsync(BasicList<CropAutoResumeModel> slots);
}