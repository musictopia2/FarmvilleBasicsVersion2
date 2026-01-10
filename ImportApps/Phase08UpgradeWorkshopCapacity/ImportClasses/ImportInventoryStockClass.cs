namespace Phase08UpgradeWorkshopCapacity.ImportClasses;
public static class ImportInventoryStockClass
{
    public static async Task ImportBeginningInventoryAmountsAsync()
    {
        BasicList<InventoryStockDocument> list = [];

        // Production (same amounts for both players)
        list.Add(Create(PlayerList.Player1, FarmThemeList.Country, ProfileIdList.Test, GetCountryProduction()));
        list.Add(Create(PlayerList.Player2, FarmThemeList.Country, ProfileIdList.Test, GetCountryProduction()));
        list.Add(Create(PlayerList.Player1, FarmThemeList.Tropical, ProfileIdList.Test, GetTropicalProduction()));
        list.Add(Create(PlayerList.Player2, FarmThemeList.Tropical, ProfileIdList.Test, GetTropicalProduction()));

        // Optional: Test profile (if you keep it around internally)
        // list.Add(Create(PlayerList.Andy, FarmThemeList.Country,  ProfileIdList.Test, GetCountryTest()));
        // list.Add(Create(PlayerList.Andy, FarmThemeList.Tropical, ProfileIdList.Test, GetTropicalTest()));

        InventoryStockDatabase db = new();
        await db.ImportAsync(list);
    }

    private static InventoryStockDocument Create(string playerName, string theme, string profileId, Dictionary<string, int> amounts)
    {
        // If FarmKey is positional record struct:
        var farm = new FarmKey(playerName, theme, profileId);

        return new InventoryStockDocument
        {
            Farm = farm,
            List = amounts
        };
    }

    private static Dictionary<string, int> GetCountryProduction()
    {
        int amount = 10;
        //eventually need to send in a list of possible options for crops.
        return new()
        {
            [CountryItemList.Wheat] = amount, //so i can craft wheat and see why i can add more when i don't have enough capacity.
            [CountryItemList.Corn] = amount,
            [CountryItemList.HoneyComb] = amount,
            [CountryItemList.Tomato] = amount,
            [CountryItemList.Strawberry] = amount,
            [CountryItemList.Carrot] = amount,
            [CurrencyKeys.Coin] = 18

        };
    }

    private static Dictionary<string, int> GetTropicalProduction()
    {
        int amount = 10;
        return new()
        {
            [TropicalItemList.Pineapple] = amount,
            [TropicalItemList.Rice] = amount,
            [TropicalItemList.Tapioca] = amount,
            [CurrencyKeys.Coin] = 18
        };
    }

    // If later you want test amounts:
    // private static Dictionary<string, int> GetCountryTest() => ...
    // private static Dictionary<string, int> GetTropicalTest() => ...
}