namespace Phase07EarnCoinFromQuests.ImportClasses;
public static class ImportCropInstanceClass
{
    // MVP1 rule: allow 20 crop slots for all farms in Production.
    // Future: Test/Casual/Hardcore profiles can use different slot counts.
    private const int _productionSlots = 20;

    public static async Task ImportCropsAsync()
    {
        BasicList<CropInstanceDocument> list = [];

        // Production farms for MVP1 (same slot count for both players and both themes)
        list.Add(CreateProduction(PlayerList.Player1, FarmThemeList.Country));
        list.Add(CreateProduction(PlayerList.Player2, FarmThemeList.Country));
        list.Add(CreateProduction(PlayerList.Player1, FarmThemeList.Tropical));
        list.Add(CreateProduction(PlayerList.Player2, FarmThemeList.Tropical));

        // Future: add other profiles/modes here
        // list.Add(CreateFarm(PlayerList.Andy, FarmThemeList.Country, ProfileIdList.Test, slots: 50));
        // list.Add(CreateFarm(PlayerList.Cristina, FarmThemeList.Tropical, ProfileIdList.Test, slots: 50));

        CropInstanceDatabase db = new();
        await db.ImportAsync(list);
    }

    private static CropInstanceDocument CreateProduction(string playerName, string theme)
        => CreateFarm(playerName, theme, ProfileIdList.Test, _productionSlots);

    private static CropInstanceDocument CreateFarm(string playerName, string theme, string profileId, int slots)
    {
        // If FarmKey is positional record struct: new FarmKey(playerName, theme, profileId)
        // If FarmKey is init-properties type: new FarmKey { PlayerName = ..., FarmTheme = ..., ProfileId = ... }
        var farm = new FarmKey
        {
            PlayerName = playerName,
            Theme = theme,
            ProfileId = profileId
        };

        return CreateInstance(farm, slots);
    }

    private static CropInstanceDocument CreateInstance(FarmKey farm, int slotCount)
    {
        BasicList<CropAutoResumeModel> slots = [];
        slotCount.Times(_ => slots.Add(new CropAutoResumeModel()));

        return new CropInstanceDocument
        {
            Farm = farm,
            Slots = slots
        };
    }

}