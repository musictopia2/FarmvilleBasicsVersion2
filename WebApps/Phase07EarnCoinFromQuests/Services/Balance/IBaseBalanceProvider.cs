using Phase07EarnCoinFromQuests.Services.Core;

namespace Phase07EarnCoinFromQuests.Services.Balance;
public interface IBaseBalanceProvider
{
    Task<BaseBalanceProfile> GetBaseBalanceAsync(FarmKey farm);
}