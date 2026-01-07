namespace Phase04PrepareForMVP1.QuestHelpers;
public static class AnimalRequirementPolicy
{

    public static TimeBandRules TimeBand => new()
    {
        MaxTimes =
        [
            TimeSpan.FromMinutes(2),
            TimeSpan.FromMinutes(5),
            TimeSpan.FromMinutes(20)
        ]
    };


    public static ActionRange Level1 => new(8, 12);
    public static ActionRange Level2 => new(5, 9);
    public static ActionRange Level3 => new(3, 7);
    public static ActionRange Level4 => new(1, 2);

    // optional global clamp after conversion to amount
    public static int MaxAmount => 40;


    public static ActionRange GetRange(TimeSpan duration)
    {
        int band = TimeBand.GetBandIndex(duration);
        return band switch
        {
            0 => Level1,
            1 => Level2,
            2 => Level3,
            _ => Level4
        };
    }
}