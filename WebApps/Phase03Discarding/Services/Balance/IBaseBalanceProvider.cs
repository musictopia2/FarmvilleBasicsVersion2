using Phase03Discarding.Services.Core;

namespace Phase03Discarding.Services.Balance;
public interface IBaseBalanceProvider
{
    Task<BaseBalanceProfile> GetBaseBalanceAsync(FarmKey farm);
}