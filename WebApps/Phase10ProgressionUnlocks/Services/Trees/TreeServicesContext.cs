namespace Phase10ProgressionUnlocks.Services.Trees;
public class TreeServicesContext
{
    required public ITreeRecipes TreeRegistry { get; init; }
    required public ITreeRepository TreeRepository { get; init; }
    required public ITreeGatheringPolicy TreeGatheringPolicy { get; init; }
    required public ITreesCollecting TreesCollecting { get; init; }
}