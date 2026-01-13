namespace Phase11StoreWithBasicPurchases.ImportClasses;
public static class ImportWorkerInstanceClass
{
    private static CatalogOfferDatabase _catalogOfferDatabase = null!;
    private static ProgressionProfileDatabase _levelProfile = null!;
    public static async Task ImportWorkersAsync()
    {
        _catalogOfferDatabase = new();
        _levelProfile = new();
        BasicList<WorkerInstanceDocument> list = [];
        var farms = FarmHelperClass.GetAllFarms();
        foreach (var farm in farms)
        {
            list.Add(await CreateInstanceAsync(farm));
        }
        WorkerInstanceDatabase db = new();
        await db.ImportAsync(list);
    }
    private static async Task<WorkerInstanceDocument> CreateInstanceAsync(FarmKey farm)
    {
        BasicList<UnlockModel> workers = [];
        var plan = await _catalogOfferDatabase.GetCatalogAsync(farm, EnumCatalogCategory.Worker);
        var profile = await _levelProfile.GetProfileAsync(farm);
        int level = profile.Level;
        foreach (var item in plan)
        {
            bool unlocked = level >= item.LevelRequired;
            workers.Add(new()
            {
                Name = item.TargetName,
                Unlocked = unlocked
            });
        }
        return new WorkerInstanceDocument
        {
            Farm = farm,
            Workers = workers
        };
    }

}
