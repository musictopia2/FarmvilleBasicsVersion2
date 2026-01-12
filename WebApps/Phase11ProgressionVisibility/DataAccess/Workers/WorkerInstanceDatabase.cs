
namespace Phase11ProgressionVisibility.DataAccess.Workers;
public class WorkerInstanceDatabase(FarmKey farm) : ListDataAccess<WorkerInstanceDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IWorkerRepository

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorkerInstances";

    async Task<BasicList<UnlockModel>> IWorkerRepository.LoadAsync()
    {
        var list = await GetDocumentsAsync();
        return list.GetSingleDocument(farm).Workers;
    }

    async Task IWorkerRepository.SaveAsync(BasicList<UnlockModel> data)
    {
        var list = await GetDocumentsAsync();
        list.GetSingleDocument(farm).Workers = data;
        await UpsertRecordsAsync(list);
    }
}