namespace Phase13QuestsBasedOnLevel.Utilities;
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
        public TimeSpan ApplyWithMinTotalForBatch(double multiplier,
        int batchSize)
        {
            if (multiplier <= 0)
            {
                throw new CustomBasicException("Time multiplier must be > 0");
            }

            if (batchSize <= 0)
            {
                throw new CustomBasicException("Batch size must be > 0");
            }

            // scaled per-unit time
            var scaledTicks = (long)Math.Round(time.Ticks * multiplier, MidpointRounding.AwayFromZero);

            // compute minimum ticks per unit so that (perUnit * batchSize) >= minTotal
            // use CEILING so the total is guaranteed to be at least minTotal
            //has to still be the 2 seconds for the total batch.
            var minPerUnitTicks = (long)Math.Ceiling(TimeSpan.FromSeconds(2).Ticks / (double)batchSize);

            if (scaledTicks < minPerUnitTicks)
            {
                scaledTicks = minPerUnitTicks;
            }

            // still guard against 0/negative ticks
            if (scaledTicks <= 0)
            {
                scaledTicks = 1;
            }

            return TimeSpan.FromTicks(scaledTicks);
        }
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
