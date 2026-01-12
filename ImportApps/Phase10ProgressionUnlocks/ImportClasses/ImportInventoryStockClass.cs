namespace Phase10ProgressionUnlocks.ImportClasses;
public static class ImportInventoryStockClass
{
    private static CropProgressionPlanDatabase _cropProgression = null!;
    private static ProgressionProfileDatabase _levelProfile = null!;
    public static async Task ImportBeginningInventoryAmountsAsync()
    {
        BasicList<InventoryStockDocument> list = [];
        var farms = FarmHelperClass.GetAllFarms();
        ProgressionProfileDatabase t = new();
        _levelProfile = new();
        _cropProgression = new();
        foreach (var farm in farms)
        {
            list.Add(await GetInventoryAsync(farm));
        }
       
        InventoryStockDatabase db = new();
        await db.ImportAsync(list);
    }

    private static async Task<InventoryStockDocument> GetInventoryAsync(FarmKey farm)
    {
        Dictionary<string, int> amounts = [];

        var p = await _levelProfile.GetProfileAsync(farm);
        int level = p.Level;
        CropProgressionPlanDocument crop = await _cropProgression.GetPlanAsync(farm);
        //var firsts = crop.UnlockRules.Where(x => level => x.LevelRequired)

        crop.UnlockRules.ForConditionalItems(x => level >= x.LevelRequired, rule =>
        {
            amounts.Add(rule.ItemName, 10);
            //amounts[rule.ItemName] = 10;
        });
        amounts.Add(CurrencyKeys.Coin, 18);

        return new()
        {
            Farm = farm,
            List = amounts
        };
    }

}