namespace Phase11ProgressionVisibility.Services.Workshops;
public interface IWorkshopRespository
{
    Task<BasicList<WorkshopAutoResumeModel>> LoadAsync();
    Task SaveAsync(BasicList<WorkshopAutoResumeModel> workshops);
}