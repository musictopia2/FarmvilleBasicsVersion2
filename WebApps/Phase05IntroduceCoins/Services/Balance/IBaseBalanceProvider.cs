using Phase05IntroduceCoins.Services.Core;

namespace Phase05IntroduceCoins.Services.Balance;
public interface IBaseBalanceProvider
{
    Task<BaseBalanceProfile> GetBaseBalanceAsync(FarmKey farm);
}