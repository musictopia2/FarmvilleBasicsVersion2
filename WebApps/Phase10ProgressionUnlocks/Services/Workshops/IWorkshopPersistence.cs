namespace Phase10ProgressionUnlocks.Services.Workshops;
public interface IWorkshopPersistence
{
    Task SaveWorkshopsAsync(BasicList<WorkshopAutoResumeModel> workshops);
}