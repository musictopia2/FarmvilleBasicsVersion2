namespace Phase07EarnCoinFromQuests.Services.Trees;
public interface ITreeProgressionPolicy
{
    Task<bool> CanUnlockTreeAsync(BasicList<TreeState> list, string name);
    Task<TreeState> UnlockTreeAsync(BasicList<TreeState> list, string name);
    Task<bool> CanLockTreeAsync(BasicList<TreeState> list, string name);
    Task<TreeState> LockTreeAsync(BasicList<TreeState> list, string name);
}