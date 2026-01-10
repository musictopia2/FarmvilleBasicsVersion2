namespace Phase06IncreaseBarnAndSiloLimits.Services.Workshops;
public interface IWorkshopProgressionPolicy
{
    Task<bool> CanUnlockAsync(BasicList<WorkshopAvailabilityState> workshops, string buildingName);
    Task<WorkshopAvailabilityState> UnlockAsync(BasicList<WorkshopAvailabilityState> workshops, string buildingName);
    Task<bool> CanLockAsync(BasicList<WorkshopAvailabilityState> workshops, string buildingName);
    Task<WorkshopAvailabilityState> LockAsync(BasicList<WorkshopAvailabilityState> workshops, string buildingName);
}