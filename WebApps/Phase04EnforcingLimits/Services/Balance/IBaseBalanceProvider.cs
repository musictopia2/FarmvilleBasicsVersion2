using Phase04EnforcingLimits.Services.Core;

namespace Phase04EnforcingLimits.Services.Balance;
public interface IBaseBalanceProvider
{
    Task<BaseBalanceProfile> GetBaseBalanceAsync(FarmKey farm);
}