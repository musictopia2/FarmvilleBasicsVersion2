namespace Phase12StoreWithBasicPurchases.Services.Workshops;
public interface IWorkshopRespository
{
    Task<BasicList<WorkshopAutoResumeModel>> LoadAsync();
    Task SaveAsync(BasicList<WorkshopAutoResumeModel> workshops);
}