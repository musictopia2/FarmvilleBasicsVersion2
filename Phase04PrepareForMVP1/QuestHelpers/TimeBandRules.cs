namespace Phase04PrepareForMVP1.QuestHelpers;

public sealed class TimeBandRules
{
    // Sorted ascending. Each threshold is the MAX time for that band.
    // Anything above the last threshold is the final band.
    public required BasicList<TimeSpan> MaxTimes { get; init; }

    public int BandCount => MaxTimes.Count + 1; // e.g. 2 thresholds => 3 bands

    public int GetBandIndex(TimeSpan t)
    {
        for (int i = 0; i < MaxTimes.Count; i++)
        {
            if (t <= MaxTimes[i])
            {
                return i; // 0..MaxTimes.Count-1
            }
        }
        return MaxTimes.Count; // last band
    }
}