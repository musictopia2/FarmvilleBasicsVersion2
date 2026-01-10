namespace Phase06IncreaseBarnAndSiloLimits.Services.Crops;
public interface ICropRegistry
{
    Task<BasicList<CropRecipe>> GetCropsAsync();

}