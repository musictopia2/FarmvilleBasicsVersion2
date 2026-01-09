namespace Phase06UpgradeBarnAndSilo.ImportClasses;
internal static class FarmHelperClass
{
    public static BasicList<FarmKey> GetAllFarms()
    {
        BasicList<FarmKey> output = [];
        BasicList<string> themes = [FarmThemeList.Country, FarmThemeList.Tropical];
        BasicList<string> players = [PlayerList.Player1, PlayerList.Player2];
        BasicList<string> profiles = [ProfileIdList.Test];
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
        return output;
    }
}
