namespace Phase04EnforcingLimits.Utilities;
public static class TimeExtensions
{
    extension (double progress)
    {
        public string GetTimeString
        {
            get
            {
                TimeSpan time = TimeSpan.FromSeconds(progress);
                return time.GetTimeString;
            }
        }
    }
    extension (TimeSpan time)
    {

        public TimeSpan Apply(double multiplier)
        {
            if (multiplier <= 0)
            {
                throw new CustomBasicException("Time multiplier must be > 0");
            }

            // preserve precision
            var ticks = (long)Math.Round(time.Ticks * multiplier, MidpointRounding.AwayFromZero);
            
            var minTicks = TimeSpan.FromSeconds(2).Ticks;

            if (ticks < minTicks)
            {
                ticks = minTicks;
            }

            return TimeSpan.FromTicks(ticks);
        }

        public string GetTimeCompact
        {
            get
            {
                if (time.TotalSeconds < 1)
                {
                    return "0s";
                }

                // Days + Hours
                if (time.TotalDays >= 1)
                {
                    return $"{time.Days}d {time.Hours}h";
                }

                // Hours + Minutes
                if (time.TotalHours >= 1)
                {
                    return $"{time.Hours}h {time.Minutes}m";
                }
                if (time.TotalMinutes >= 1)
                {
                    // Minutes + Seconds
                    return $"{time.Minutes}m {time.Seconds}s";
                }
                return $"{time.Seconds}s";
            }
        }
        public string GetTimeString
        {
            get
            {
                if (time.TotalSeconds < 1)
                {
                    return "0s";
                }

                var parts = new BasicList<string>();

                if (time.Days > 0)
                {
                    parts.Add($"{time.Days}d");
                }

                if (time.Hours > 0)
                {
                    parts.Add($"{time.Hours}h");
                }

                if (time.Minutes > 0)
                {
                    parts.Add($"{time.Minutes}m");
                }

                // Only show seconds if:
                // - there are no larger units, OR
                // - seconds > 0
                if (time.Seconds > 0 || parts.Count == 0)
                {
                    parts.Add($"{time.Seconds}s");
                }

                return string.Join(" ", parts);
            }
        }
    }
}
