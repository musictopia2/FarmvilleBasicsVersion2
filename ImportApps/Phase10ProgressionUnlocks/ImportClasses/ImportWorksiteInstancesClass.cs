namespace Phase10ProgressionUnlocks.ImportClasses;
public static class ImportWorksiteInstancesClass
{
    private static BasicList<WorksiteRecipeDocument> _recipes = [];
    private static WorksiteProgressionPlanDatabase _worksiteProgression = null!;
    private static ProgressionProfileDatabase _levelProfile = null!;
    public static async Task ImportWorksitesAsync()
    {
        _worksiteProgression = new();
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
        var worksitePlan = await _worksiteProgression.GetPlanAsync(farm);
        var profile = await _levelProfile.GetProfileAsync(farm);
        int level = profile.Level;
        foreach (var item in worksitePlan.UnlockRules)
        {
            bool unlocked = level >= item.LevelRequired;
            worksites.Add(new()
            {
                Name = item.ItemName,
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