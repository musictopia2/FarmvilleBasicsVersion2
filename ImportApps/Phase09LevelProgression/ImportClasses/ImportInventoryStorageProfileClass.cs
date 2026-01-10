namespace Phase09LevelProgression.ImportClasses;
public static class ImportInventoryStorageProfileClass
{
    public static async Task ImportInventoryProfilesAsync()
    {
        //for this version, do the same for both players.
        var farms = FarmHelperClass.GetAllFarms();
        InventoryStorageUpgradePlanDatabase others = new();
        
        InventoryStorageProfileDatabase db = new();
        BasicList<InventoryStorageProfileDocument> list = [];
        foreach (var farm in farms)
        {
            var upgrades = await others.GetUpgradesAsync(farm);
            InventoryStorageProfileDocument document = new()
            {
                Farm = farm,
                BarnLevel = 0,
                SiloLevel = 0,
                BarnSize = upgrades.BarnUpgrades.First().Size,
                SiloSize = upgrades.SiloUpgrades.First().Size
            };
            list.Add(document);
        }
        await db.ImportAsync(list);
    }
}