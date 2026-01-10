namespace Phase06IncreaseBarnAndSiloLimits.Services.Workshops;
public interface IWorkshopCollectionPolicy
{
    Task<bool> IsAutomaticAsync();
}