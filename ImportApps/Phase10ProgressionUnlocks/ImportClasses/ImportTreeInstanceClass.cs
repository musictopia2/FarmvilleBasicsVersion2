namespace Phase10ProgressionUnlocks.ImportClasses;
public static class ImportTreeInstanceClass
{
    private static TreeProgressionPlanDatabase _treeProgression = null!;
    private static ProgressionProfileDatabase _levelProfile = null!;
    public static async Task ImportTreesAsync()
    {
        _treeProgression = new();
        _levelProfile = new();
        BasicList<TreeInstanceDocument> list = [];
        var farms = FarmHelperClass.GetAllFarms();
        foreach ( var farm in farms )
        {
            list.Add(await CreateInstanceAsync(farm));
        }
        TreeInstanceDatabase db = new();
        await db.ImportAsync(list);
    }
    private static async Task<TreeInstanceDocument> CreateInstanceAsync(FarmKey farm)
    {
        BasicList<TreeAutoResumeModel> trees = [];
        var treelPlan = await _treeProgression.GetPlanAsync(farm);
        var profile = await _levelProfile.GetProfileAsync(farm);
        int level = profile.Level;
        foreach (var item in treelPlan.UnlockRules)
        {
            bool unlocked = level >= item.LevelRequired;
            trees.Add(new()
            {
                TreeName = item.ItemName,
                Unlocked = unlocked
            });
        }
        return new TreeInstanceDocument
        {
            Farm = farm,
            Trees = trees
        };
    }

}