namespace Phase07EarnCoinFromQuests.Services.Workshops;
public interface IWorkshopInstances
{
    Task<BasicList<WorkshopAutoResumeModel>> GetWorkshopInstancesAsync();
}