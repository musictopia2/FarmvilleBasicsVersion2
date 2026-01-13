namespace Phase12StoreWithBasicPurchases.DataAccess.Workshops;

internal class WorkshopInstanceDatabase(FarmKey farm) : ListDataAccess<WorkshopInstanceDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IWorkshopRespository

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorkshopInstances";

    async Task<BasicList<WorkshopAutoResumeModel>> IWorkshopRespository.LoadAsync()
    {
        var firsts = await GetDocumentsAsync();
        BasicList<WorkshopAutoResumeModel> output = firsts.GetSingleDocument(farm).Workshops;
        return output;
    }

    async Task IWorkshopRespository.SaveAsync(BasicList<WorkshopAutoResumeModel> workshops)
    {
        var list = await GetDocumentsAsync();
        var item = list.GetSingleDocument(farm);
        item.Workshops = workshops;
        await UpsertRecordsAsync(list);
    }

}