namespace Phase06IncreaseBarnAndSiloLimits.Services.Core;
public interface IStartFarmRegistry
{
    Task<BasicList<FarmKey>> GetFarmsAsync(); 
}