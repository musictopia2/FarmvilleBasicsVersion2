namespace Phase05BarnSiloLimits.ImportClasses;
public static class ImportStartClass
{
    public static async Task ImportStartAsync()
    {
        BasicList<FarmKey> output = [];
        BasicList<string> themes = [FarmThemeList.Country, FarmThemeList.Tropical];
        BasicList<string> players = [PlayerList.Andy, PlayerList.Cristina];
        BasicList<string> profiles = [ProfileIdList.Production];
        foreach (var player in players)
        {
            foreach (var theme in themes)
            {
                foreach (var profile in profiles)
                {
                    output.Add(new FarmKey()
                    {
                        PlayerName = player,
                        Theme = theme,
                        ProfileId = profile
                    });
                }
            }
        }
        StartFarmDatabase db = new();
        await db.ImportAsync(output);
    }
}