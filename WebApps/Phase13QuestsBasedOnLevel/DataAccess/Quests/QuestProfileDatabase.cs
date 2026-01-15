
namespace Phase13QuestsBasedOnLevel.DataAccess.Quests;
public class QuestProfileDatabase(FarmKey farm) : ListDataAccess<QuestProfileDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IQuestProfile

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "QuestProfile";
    async Task<BasicList<QuestInstanceModel>> IQuestProfile.LoadAsync()
    {
        var list = await GetDocumentsAsync();
        return list.GetSingleDocument(farm).Quests;
    }
    async Task IQuestProfile.SaveAsync(BasicList<QuestInstanceModel> quests)
    {
        var list = await GetDocumentsAsync();
        list.GetSingleDocument(farm).Quests = quests;
        await UpsertRecordsAsync(list);
    }
}