namespace Phase06IncreaseBarnAndSiloLimits.DataAccess.Workshops;
public class BasicWorkshopPolicy : IWorkshopProgressionPolicy
{
    Task<bool> IWorkshopProgressionPolicy.CanLockAsync(BasicList<WorkshopAvailabilityState> workshops, string buildingName)
    {
        return Task.FromResult(false);
    }

    Task<bool> IWorkshopProgressionPolicy.CanUnlockAsync(BasicList<WorkshopAvailabilityState> workshops, string buildingName)
    {
        return Task.FromResult(false);
    }

    Task<WorkshopAvailabilityState> IWorkshopProgressionPolicy.LockAsync(BasicList<WorkshopAvailabilityState> workshops, string buildingName)
    {
        throw new NotImplementedException();
    }

    Task<WorkshopAvailabilityState> IWorkshopProgressionPolicy.UnlockAsync(BasicList<WorkshopAvailabilityState> workshops, string buildingName)
    {
        throw new NotImplementedException();
    }
}
