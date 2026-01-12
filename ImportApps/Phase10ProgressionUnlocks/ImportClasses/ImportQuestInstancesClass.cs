namespace Phase10ProgressionUnlocks.ImportClasses;
public static class ImportQuestInstancesClass
{
    public static async Task ImportQuestsAsync()
    {
        QuestInstanceDatabase db = new();
        BasicList<QuestDocument> list = [];
        // MVP needs quests for BOTH players across BOTH themes
        var players = new[] { PlayerList.Player1, PlayerList.Player2 };
        var themes = new[] { FarmThemeList.Country, FarmThemeList.Tropical };
        foreach (var player in players)
        {
            foreach (var theme in themes)
            {
                FarmKey farm = GetFarm(player, theme);

                BasicList<QuestRecipe> quests = await CompleteQuestClass.GetQuestsAsync(farm);
                QuestDocument document = new()
                {
                    Farm = farm,
                    Quests = quests
                };
                //list.Add(document);
            }
        }
        await db.ImportAsync(list);
    }

    private static FarmKey GetFarm(string playerName, string theme)
    {
        return new()
        {
            Theme = theme,
            PlayerName = playerName,
            ProfileId = ProfileIdList.Test
        };
    }

}