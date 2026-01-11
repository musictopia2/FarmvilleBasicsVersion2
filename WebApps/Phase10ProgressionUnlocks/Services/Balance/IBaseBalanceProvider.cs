using Phase10ProgressionUnlocks.Services.Core;

namespace Phase10ProgressionUnlocks.Services.Balance;
public interface IBaseBalanceProvider
{
    Task<BaseBalanceProfile> GetBaseBalanceAsync(FarmKey farm);
}