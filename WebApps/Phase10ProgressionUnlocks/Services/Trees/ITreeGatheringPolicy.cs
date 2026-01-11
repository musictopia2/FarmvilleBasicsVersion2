namespace Phase10ProgressionUnlocks.Services.Trees;

public interface ITreeGatheringPolicy
{
    /// <summary>
    /// True = collect all stored fruit
    /// False = collect one fruit per click
    /// </summary>
    Task<bool> CollectAllAsync();
}