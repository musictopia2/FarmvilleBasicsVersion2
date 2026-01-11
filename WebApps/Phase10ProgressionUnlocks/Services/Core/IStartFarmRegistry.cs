namespace Phase10ProgressionUnlocks.Services.Core;
public interface IStartFarmRegistry
{
    Task<BasicList<FarmKey>> GetFarmsAsync(); 
}