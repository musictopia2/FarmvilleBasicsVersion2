namespace Phase08UpgradeWorkshopCapacity.Services.Crops;
public interface ICropRegistry
{
    Task<BasicList<CropRecipe>> GetCropsAsync();

}