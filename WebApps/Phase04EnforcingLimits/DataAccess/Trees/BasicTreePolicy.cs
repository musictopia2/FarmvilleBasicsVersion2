namespace Phase04EnforcingLimits.DataAccess.Trees;
public class BasicTreePolicy : ITreeProgressionPolicy
{
    Task<bool> ITreeProgressionPolicy.CanLockTreeAsync(BasicList<TreeState> list, string name)
    {
        return Task.FromResult(false);
    }

    Task<bool> ITreeProgressionPolicy.CanUnlockTreeAsync(BasicList<TreeState> list, string name)
    {
        return Task.FromResult(false);
    }

    Task<TreeState> ITreeProgressionPolicy.LockTreeAsync(BasicList<TreeState> list, string name)
    {
        throw new NotImplementedException();
    }

    Task<TreeState> ITreeProgressionPolicy.UnlockTreeAsync(BasicList<TreeState> list, string name)
    {
        throw new NotImplementedException();
    }
}