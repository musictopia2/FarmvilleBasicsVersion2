using CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator; //not common enough.
using rs1 = CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator.RandomHelpers; //not common enough.

//we put here instead of data access because preparing for complex processes (future).

namespace Phase14MVP2.RandomQuestGeneratorProcesses;

public class RandomQuestGenerationService(CropManager cropManager,
    TreeManager treeManager
    ) : IQuestGenerationService
{

    //this version will be very simple.
    // Phase 13: simple anti-dup logic. You can tune later.
    private const int _duplicateCap = 3; //suggested no more than 3 at a time.
    private readonly IRandomNumberList _rng = rs1.GetRandomGenerator();

    QuestInstanceModel IQuestGenerationService.CreateQuest(
    int currentLevel,
    BasicList<ItemPlanModel> eligibleItems,
    BasicList<QuestInstanceModel> existingBoard)
    {
        if (eligibleItems.Count == 0)
        {
            throw new CustomBasicException($"No eligible quest items for level {currentLevel}.");
        }

        if (currentLevel < 5)
        {
            var chosenEarly = eligibleItems.GetRandomItem();
            return BuildQuest(chosenEarly, currentLevel);
        }

        var counts = existingBoard
            .Where(q => q.Status == EnumQuestStatus.Active)
            .GroupBy(q => q.ItemName)
            .ToDictionary(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);

        string? lastItem = existingBoard.Count == 0 ? null : existingBoard[^1].ItemName;

        // 1) Prefer items not currently on the board
        var candidates = eligibleItems
            .Where(i => !counts.ContainsKey(i.ItemName))
            .ToBasicList();

        // 2) If none, allow duplicates up to cap
        if (candidates.Count == 0)
        {
            candidates = eligibleItems
                .Where(i => !counts.TryGetValue(i.ItemName, out int c) || c < _duplicateCap)
                .ToBasicList();
        }

        // 3) If still none, allow anything
        if (candidates.Count == 0)
        {
            candidates = eligibleItems;
        }

        // 4) Prefer not repeating the immediately last item (only if possible)
        if (lastItem is not null)
        {
            var withoutLast = candidates
                .Where(i => !i.ItemName.Equals(lastItem, StringComparison.OrdinalIgnoreCase))
                .ToBasicList();

            if (withoutLast.Count > 0)
            {
                candidates = withoutLast;
            }
        }

        var chosen = candidates.GetRandomItem();
        return BuildQuest(chosen, currentLevel);
    }

    private int NextInt(int minInclusive, int maxInclusive)
    => _rng.GetRandomNumber(maxInclusive, minInclusive);

    private int GetRequired(ItemPlanModel chosen) => chosen.Category switch
    {
        EnumItemCategory.Worksite => NextInt(1, 2),
        EnumItemCategory.Crop => GetCropCount(chosen),
        EnumItemCategory.Tree => GetTreeCount(chosen),
        EnumItemCategory.Animal => NextInt(6, 10),
        _ => NextInt(3, 8),
    };

    private int GetCropCount(ItemPlanModel chosen)
    {
        TimeSpan duration = cropManager.GetTimeForGivenCrop(chosen.ItemName);
        if (duration.TotalMinutes < 10)
        {
            return NextInt(20, 50);
        }
        return NextInt(10, 15); //try this way now.
    }
    private int GetTreeCount(ItemPlanModel chosen)
    {
        TimeSpan duration = treeManager.GetTimeForGivenTree(chosen.ItemName);
        if (duration.TotalMinutes < 10)
        {
            return NextInt(10, 16);
        }
        return NextInt(3, 8);
    }

    private QuestInstanceModel BuildQuest(ItemPlanModel chosen, int level) => new()
    {
        ItemName = chosen.ItemName,
        Rewards = CostUtilities.GetCoinOnlyDictionary(1),
        LevelRequired = level,
        Required = GetRequired(chosen)
    };

}
