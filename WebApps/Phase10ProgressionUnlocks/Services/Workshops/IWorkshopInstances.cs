namespace Phase10ProgressionUnlocks.Services.Workshops;
public interface IWorkshopInstances
{
    Task<BasicList<WorkshopAutoResumeModel>> GetWorkshopInstancesAsync();
}