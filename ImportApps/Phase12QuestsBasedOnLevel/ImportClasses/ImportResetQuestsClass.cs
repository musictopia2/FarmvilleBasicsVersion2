namespace Phase12QuestsBasedOnLevel.ImportClasses;

public static class ImportResetQuestsClass
{
    public static async Task ResetQuestsAsync()
    {
        var farms = FarmHelperClass.GetAllFarms();
        BasicList<QuestProfileDocument> list = [];
        foreach (var farm in farms)
        {
            QuestProfileDocument document = new()
            {
                Farm = farm,
                Quests = []
            };
            list.Add(document);
        }
        QuestProfileDatabase db = new();
        await db.ImportAsync(list);
    }
}