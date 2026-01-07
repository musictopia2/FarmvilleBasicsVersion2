namespace Phase04PrepareForMVP1.QuestHelpers;
public static class TreeRequirementPolicy
{
    public static TimeBandRules TimeBand => new()
    {
        MaxTimes =
       [
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(5),
            TimeSpan.FromMinutes(30),
            TimeSpan.FromHours(1),
            TimeSpan.FromHours(2)
       ]
    };

    //public TimeBandRules BandRules { get; init; } = new()
    //{
    //    EasyMax = TimeSpan.FromMinutes(5),
    //    ModerateMax = TimeSpan.FromMinutes(30)
    //};
    public static ActionRange Level0 => new(18, 30);
    public static ActionRange Level1 => new(12, 16);
    public static ActionRange Level2 => new(8, 13);

    public static ActionRange Level3 => new(6, 12);

    public static ActionRange Level4 => new(3, 6);
    public static ActionRange Level5 => new(2, 4);

    // optional global clamp after conversion to amount
    public static int MaxAmount => 30;



    public static ActionRange GetRange(TimeSpan duration)
    {
        int band = TimeBand.GetBandIndex(duration);
        return band switch
        {
            0 => Level0,
            1 => Level1,
            2 => Level2,
            3 => Level3,
            4 => Level4,
            _ => Level5
        };
    }

}
