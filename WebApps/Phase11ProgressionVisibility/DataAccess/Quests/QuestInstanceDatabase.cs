using Phase11ProgressionVisibility.Services.Core;

namespace Phase11ProgressionVisibility.DataAccess.Quests;
public class QuestInstanceDatabase(FarmKey farm) : ListDataAccess<QuestDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IQuestRecipes, IQuestPersistence

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "QuestInstances";
    async Task<BasicList<QuestRecipe>> IQuestRecipes.GetQuestsAsync()
    {
        return []; //for now, no quests.
        //var firsts = await GetDocumentsAsync();
        //BasicList<QuestRecipe> output = firsts.singledo).Quests;
        //return output;
    }
    async Task IQuestPersistence.SaveQuestsAsync(BasicList<QuestRecipe> quests)
    {
        var list = await GetDocumentsAsync();
        var item = list.GetSingleDocument(farm);
        item.Quests = quests;
        await UpsertRecordsAsync(list);
    }
}