namespace Phase07EarnCoinFromQuests.Services.Workshops;
public interface IWorkshopPersistence
{
    Task SaveWorkshopsAsync(BasicList<WorkshopAutoResumeModel> workshops);
}