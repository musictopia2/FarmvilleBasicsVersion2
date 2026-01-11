namespace Phase10ProgressionUnlocks.Services.Workshops;
public interface IWorkshopCollectionPolicy
{
    Task<bool> IsAutomaticAsync();
}