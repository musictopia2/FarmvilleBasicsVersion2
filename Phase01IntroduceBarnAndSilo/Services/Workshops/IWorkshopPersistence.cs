namespace Phase01IntroduceBarnAndSilo.Services.Workshops;
public interface IWorkshopPersistence
{
    Task SaveWorkshopsAsync(BasicList<WorkshopAutoResumeModel> workshops);
}