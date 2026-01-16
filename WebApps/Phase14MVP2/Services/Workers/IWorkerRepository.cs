namespace Phase14MVP2.Services.Workers;
public interface IWorkerRepository
{
    Task<BasicList<UnlockModel>> LoadAsync();
    Task SaveAsync(BasicList<UnlockModel> data);
}