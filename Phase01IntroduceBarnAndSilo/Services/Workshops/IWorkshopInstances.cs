namespace Phase01IntroduceBarnAndSilo.Services.Workshops;
public interface IWorkshopInstances
{
    Task<BasicList<WorkshopAutoResumeModel>> GetWorkshopInstancesAsync();
}