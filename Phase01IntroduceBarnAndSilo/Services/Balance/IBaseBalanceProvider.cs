using Phase01IntroduceBarnAndSilo.Services.Core;

namespace Phase01IntroduceBarnAndSilo.Services.Balance;
public interface IBaseBalanceProvider
{
    Task<BaseBalanceProfile> GetBaseBalanceAsync(FarmKey farm);
}