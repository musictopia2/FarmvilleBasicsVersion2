namespace Phase04PrepareForMVP1.QuestHelpers;

internal static class WorksiteRequirementPolicy
{
    public static TimeSpan TargetTotalTime => TimeSpan.FromHours(1);

    // Your “runs per 1 expected item” vision
    public static double RunsPerItem_Common => 2.0;
    public static double RunsPerItem_Rare => 10.0;

    // Run caps
    public static int MinRuns => 1;
    public static int MaxRuns_Guaranteed => 6;
    public static int MaxRuns_Common => 4;
    public static int MaxRuns_Rare => 2;

    // Final amount caps
    public static int MaxAmount => 10;
    public static int RareMaxAmount => 2;

    // Demand penalty (used by workshop recipes)
    public static int DemandThreshold1 => 2;
    public static int DemandThreshold2 => 5;
    public static int DemandPenalty1 => 1;
    public static int DemandPenalty2 => 2;
}