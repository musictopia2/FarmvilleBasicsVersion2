using Phase14MVP2.Services.Core;

namespace Phase14MVP2.Services.Balance;
public interface IBaseBalanceProvider
{
    Task<BaseBalanceProfile> GetBaseBalanceAsync(FarmKey farm);
}