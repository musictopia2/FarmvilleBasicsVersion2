namespace Phase04PrepareForMVP1.QuestHelpers;
public static class CropRequirementPolicy
{
    public static TimeBandRules TimeBand => new()
    {
        MaxTimes =
       [
            TimeSpan.FromMinutes(3),
            TimeSpan.FromMinutes(14),
            TimeSpan.FromMinutes(30),
            TimeSpan.FromHours(1),
            TimeSpan.FromHours(2)
       ]
    };
    public static ActionRange Level1 => new(24, 40);
    public static ActionRange Level2 => new(18, 30);
    public static ActionRange Level3 => new(12, 16);
    public static ActionRange Level4 => new(9, 13);
    public static ActionRange Level5 => new(4, 8);
    public static ActionRange Level6 => new(2, 3);
    public static ActionRange GetRange(TimeSpan duration)
    {
        int band = TimeBand.GetBandIndex(duration);
        return band switch
        {
            0 => Level1,
            1 => Level2,
            2 => Level3,
            3 => Level4,
            _ => Level5
        };
    }
    public static int MaxAmount => 30;
}