using Phase09LevelProgression.Services.Core;

namespace Phase09LevelProgression.Services.Balance;
public interface IBaseBalanceProvider
{
    Task<BaseBalanceProfile> GetBaseBalanceAsync(FarmKey farm);
}