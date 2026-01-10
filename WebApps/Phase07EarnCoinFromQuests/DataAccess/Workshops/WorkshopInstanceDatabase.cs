using Phase07EarnCoinFromQuests.Services.Core;

namespace Phase07EarnCoinFromQuests.DataAccess.Workshops;
internal class WorkshopInstanceDatabase(FarmKey farm) : ListDataAccess<WorkshopInstanceDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IWorkshopInstances, IWorkshopPersistence

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorkshopInstances";
    async Task<BasicList<WorkshopAutoResumeModel>> IWorkshopInstances.GetWorkshopInstancesAsync()
    {
        var firsts = await GetDocumentsAsync();
        BasicList<WorkshopAutoResumeModel> output = firsts.Single(x => x.Farm.Equals(farm)).Workshops;
        return output;
    }
    async Task IWorkshopPersistence.SaveWorkshopsAsync(BasicList<WorkshopAutoResumeModel> workshops)
    {
        var list = await GetDocumentsAsync();
        var item = list.Single(x => x.Farm.Equals(farm));
        item.Workshops = workshops;
        await UpsertRecordsAsync(list);
    }
}