namespace Phase11StoreWithBasicPurchases.ImportClasses;
public static class ImportWorksiteInstancesClass
{
    private static CatalogOfferDatabase _catalogOfferDatabase = null!;
    private static ProgressionProfileDatabase _levelProfile = null!;
    public static async Task ImportWorksitesAsync()
    {
        _catalogOfferDatabase = new();
        _levelProfile = new();
        BasicList<WorksiteInstanceDocument> list = [];
        var farms = FarmHelperClass.GetAllFarms();
        foreach (var farm in farms)
        {
            list.Add(await CreateInstanceAsync(farm));
        }
        WorksiteInstanceDatabase db = new();
        await db.ImportAsync(list);
    }
    private static async Task<WorksiteInstanceDocument> CreateInstanceAsync(FarmKey farm)
    {
        BasicList<WorksiteAutoResumeModel> worksites = [];
        var plan = await _catalogOfferDatabase.GetCatalogAsync(farm, EnumCatalogCategory.Worksite);
        var profile = await _levelProfile.GetProfileAsync(farm);
        int level = profile.Level;
        foreach (var item in plan)
        {
            bool unlocked = level >= item.LevelRequired;
            worksites.Add(new()
            {
                Name = item.TargetName,
                Unlocked = unlocked
            });
        }
        return new WorksiteInstanceDocument
        {
            Farm = farm,
            Worksites = worksites
        };
    }
}