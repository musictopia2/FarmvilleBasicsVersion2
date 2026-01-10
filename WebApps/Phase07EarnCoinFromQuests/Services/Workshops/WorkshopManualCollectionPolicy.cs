namespace Phase07EarnCoinFromQuests.Services.Workshops;
public class WorkshopManualCollectionPolicy : IWorkshopCollectionPolicy
{
    Task<bool> IWorkshopCollectionPolicy.IsAutomaticAsync()
    {
        return Task.FromResult(false);
    }
}