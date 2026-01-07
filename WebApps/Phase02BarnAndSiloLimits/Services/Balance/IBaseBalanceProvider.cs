using Phase02BarnAndSiloLimits.Services.Core;

namespace Phase02BarnAndSiloLimits.Services.Balance;
public interface IBaseBalanceProvider
{
    Task<BaseBalanceProfile> GetBaseBalanceAsync(FarmKey farm);
}