namespace Phase10ProgressionUnlocks.ImportClasses;
public static class ImportCropInstanceClass
{

    private static CropProgressionPlanDatabase _cropProgression = null!;
    private static ProgressionProfileDatabase _levelProfile = null!;
    public static async Task ImportCropsAsync()
    {
        BasicList<CropInstanceDocument> list = [];
        _cropProgression = new();
        _levelProfile = new();
        // Production farms for MVP1 (same slot count for both players and both themes)

        var firsts = FarmHelperClass.GetAllFarms();
        foreach (var farm in firsts)
        {
            list.Add(await CreateInstanceAsync(farm));
        }

        CropInstanceDatabase db = new();
        await db.ImportAsync(list);
    }


    private static async Task<CropInstanceDocument> CreateInstanceAsync (FarmKey farm)
    {
        BasicList<CropAutoResumeModel> slots = [];
        BasicList<CropDataModel> crops = [];
        var cropPlan = await _cropProgression.GetPlanAsync(farm);

        var profile = await _levelProfile.GetProfileAsync(farm);


        foreach (var level in cropPlan.SlotLevelRequired)
        {
            bool unlocked = true;
            if (level > profile.Level)
            {
                unlocked = false;
            }
            CropAutoResumeModel slot = new()
            {
                Unlocked = unlocked,
            };
            slots.Add(slot);
        }
        foreach (var item in cropPlan.UnlockRules)
        {
            bool unlocked = true;
            if (item.LevelRequired > profile.Level)
            {
                unlocked = false;
            }
            CropDataModel crop = new()
            {
                Item = item.ItemName,
                Unlocked = unlocked,
            };
            crops.Add(crop);
        }

        CropInstanceDocument output = new()
        {
            Farm = farm,
            Crops = crops,
            Slots = slots

        };
        return output;

    }

}