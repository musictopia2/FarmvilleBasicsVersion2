namespace Phase10ProgressionUnlocks.Services.Crops;
public interface ICropRegistry
{
    Task<BasicList<CropRecipe>> GetCropsAsync();

}