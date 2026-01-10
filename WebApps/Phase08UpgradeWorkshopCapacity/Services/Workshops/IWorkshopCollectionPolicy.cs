namespace Phase08UpgradeWorkshopCapacity.Services.Workshops;
public interface IWorkshopCollectionPolicy
{
    Task<bool> IsAutomaticAsync();
}