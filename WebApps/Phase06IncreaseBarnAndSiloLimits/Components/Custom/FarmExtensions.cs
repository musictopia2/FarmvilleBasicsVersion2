namespace Phase06IncreaseBarnAndSiloLimits.Components.Custom;
public static class FarmExtensions
{
    extension(FarmComponentBase farm)
    {
        public bool CanCloseWorksiteAutomatically(string? location)
        {
            return farm.WorksiteManager.CanCloseWorksiteAutomatically(location);

        }
    }
    extension(FarmContext farm)
    {
        public bool CanCloseWorksiteAutomatically(string? location)
        {
            return farm.Current!.WorksiteManager.CanCloseWorksiteAutomatically(location);

        }
    }
    extension(WorksiteManager manager)
    {
        public bool CanCloseWorksiteAutomatically(string? location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return true; //try this way (?)
            }
            EnumWorksiteState status = manager.GetStatus(location);
            if (status != EnumWorksiteState.Collecting)
            {
                return true;
            }
            if (manager.CanCollectRewards(location))
            {
                if (manager.CanCollectRewardsWithLimits(location) == false)
                {
                    return true; //because you have limits
                }
                return false;
            }
            return true;
        }
    }
}
