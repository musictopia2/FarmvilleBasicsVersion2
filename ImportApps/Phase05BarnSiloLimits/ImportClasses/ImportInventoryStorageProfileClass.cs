namespace Phase05BarnSiloLimits.ImportClasses;
public static class ImportInventoryStorageProfileClass
{
    public static async Task ImportInventoryProfilesAsync()
    {
        //for this version, do the same for both players.
        var farms = FarmHelperClass.GetAllFarms();

        InventoryStorageProfileDatabase db = new();
        BasicList<InventoryStorageProfileDocument> list = [];
        foreach (var farm in farms)
        {
            InventoryStorageProfileDocument document = new()
            {
                Farm = farm,
                BarnSize = 2,
                SiloSize = 20
            };
            list.Add(document);
        }
        //just to prove it shows limits and you will quickly exceed them.
        await db.ImportAsync(list);



    }
}
