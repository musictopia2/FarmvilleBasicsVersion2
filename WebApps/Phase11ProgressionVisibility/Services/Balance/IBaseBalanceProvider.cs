using Phase11ProgressionVisibility.Services.Core;

namespace Phase11ProgressionVisibility.Services.Balance;
public interface IBaseBalanceProvider
{
    Task<BaseBalanceProfile> GetBaseBalanceAsync(FarmKey farm);
}