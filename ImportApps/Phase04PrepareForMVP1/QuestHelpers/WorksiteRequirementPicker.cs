namespace Phase04PrepareForMVP1.QuestHelpers;

internal static class WorksiteRequirementPicker
{

    private readonly static IRandomNumberList _rs;
    static WorksiteRequirementPicker()
    {
        _rs = rs1.GetRandomGenerator();
    }
    public static int PickRequiredAmount(WorksiteOutputAnalysis a)
    {
        int expectedPerRun = Math.Max(1, a.OutputAmount);

        // 1) base runs from time
        double mins = Math.Max(1.0, a.Duration.TotalMinutes);
        double baseRuns = WorksiteRequirementPolicy.TargetTotalTime.TotalMinutes / mins;

        // 2) convert baseRuns into a random window (±15%)
        int minRuns = (int)Math.Floor(baseRuns * 0.85);
        int maxRuns = (int)Math.Ceiling(baseRuns * 1.15);

        if (minRuns < WorksiteRequirementPolicy.MinRuns)
        {
            minRuns = WorksiteRequirementPolicy.MinRuns;
        }

        if (maxRuns < minRuns)
        {
            maxRuns = minRuns;
        }

        // 3) pick runs using YOUR RNG (max first, then min)
        int runs = _rs.GetRandomNumber(maxRuns, minRuns);

        runs = ClampRunsByRarity(runs, a.Rarity);

        // 4) convert runs -> expected amount
        double baseAmount = BaseAmountFromRuns(a.Rarity, runs, expectedPerRun);
        int amount = (int)Math.Round(baseAmount);

        // 5) reduce if item is a workshop bottleneck
        amount -= DemandPenalty(a.FromWorkshops);
        if (amount < 1)
        {
            amount = 1;
        }

        // 6) mercy caps
        if (a.Rarity == EnumWorksiteRarity.Rare)
        {
            amount = Math.Min(amount, WorksiteRequirementPolicy.RareMaxAmount);
        }

        amount = Math.Min(amount, WorksiteRequirementPolicy.MaxAmount);

        return amount;
    }
    private static double BaseAmountFromRuns(EnumWorksiteRarity rarity, int runs, int expectedPerRun)
    {
        // Guaranteed: you get it each run (OutputAmount already reflects “send all workers”)
        if (rarity == EnumWorksiteRarity.Guaranteed)
        {
            return runs * expectedPerRun;
        }

        // If None (shouldn't usually happen), treat like Common
        double rpi = (rarity == EnumWorksiteRarity.Rare)
            ? WorksiteRequirementPolicy.RunsPerItem_Rare
            : WorksiteRequirementPolicy.RunsPerItem_Common;

        return (runs / rpi) * expectedPerRun;
    }

    private static int DemandPenalty(int fromWorkshops)
    {
        if (fromWorkshops >= WorksiteRequirementPolicy.DemandThreshold2)
        {
            return WorksiteRequirementPolicy.DemandPenalty2;
        }

        if (fromWorkshops >= WorksiteRequirementPolicy.DemandThreshold1)
        {
            return WorksiteRequirementPolicy.DemandPenalty1;
        }

        return 0;
    }

    private static int ClampRunsByRarity(int runs, EnumWorksiteRarity r)
    {
        runs = Math.Max(WorksiteRequirementPolicy.MinRuns, runs);

        int max = r switch
        {
            EnumWorksiteRarity.Guaranteed => WorksiteRequirementPolicy.MaxRuns_Guaranteed,
            EnumWorksiteRarity.Common => WorksiteRequirementPolicy.MaxRuns_Common,
            EnumWorksiteRarity.Rare => WorksiteRequirementPolicy.MaxRuns_Rare,
            _ => WorksiteRequirementPolicy.MaxRuns_Common
        };

        return Math.Min(runs, max);
    }
}