namespace Phase05BarnSiloLimits.ImportClasses;
public static class ImportBalanceMultiplierClass
{
    public static async Task ImportBalanceMultiplierAsync()
    {
        BasicList<BalanceProfileDocument> list = [];

        // MVP1 Production:
        // - All activities take 50% of their base recipe time
        list.Add(CreateFarm(PlayerList.Andy, FarmThemeList.Country, ProfileIdList.Production, 0.5, treeOverride: 0.25, worksiteOverride: 0.3333));
        list.Add(CreateFarm(PlayerList.Cristina, FarmThemeList.Country, ProfileIdList.Production, 0.5, treeOverride: 0.25,  worksiteOverride: 0.3333));
        list.Add(CreateFarm(PlayerList.Andy, FarmThemeList.Tropical, ProfileIdList.Production, 0.5, treeOverride: 0.25, worksiteOverride: 0.33333));
        list.Add(CreateFarm(PlayerList.Cristina, FarmThemeList.Tropical, ProfileIdList.Production, 0.5, treeOverride: 0.25, worksiteOverride: 0.33333));

        //this shows an example of how to override the workshop.   means the workshop won't be as fast as the rest.
        //list.Add(CreateFarm(
        //    PlayerList.Andy,
        //    FarmThemeList.Country,
        //    ProfileIdList.Production,
        //    baseMultiplier: 0.5,
        //    workshopOverride: 0.75
        //));

        list.ForEach(Validate);
        BalanceProfileDatabase db = new();
        await db.ImportAsync(list);
    }

    // MVP1 default: half the published recipe time
    // Example:
    //  - 1 hour recipe -> 30 minutes
    //  - 10 seconds recipe -> 5 seconds
    private static BalanceProfileDocument CreateProduction(string playerName, string theme)
        => CreateFarm(
            playerName,
            theme,
            ProfileIdList.Production,
            baseMultiplier: 0.5
        );

    private static void Validate(BalanceProfileDocument b)
    {
        ValidateOne(b.CropTimeMultiplier, nameof(b.CropTimeMultiplier), b.Farm);
        ValidateOne(b.AnimalTimeMultiplier, nameof(b.AnimalTimeMultiplier), b.Farm);
        ValidateOne(b.WorkshopTimeMultiplier, nameof(b.WorkshopTimeMultiplier), b.Farm);
        ValidateOne(b.TreeTimeMultiplier, nameof(b.TreeTimeMultiplier), b.Farm);
        ValidateOne(b.WorksiteTimeMultiplier, nameof(b.WorksiteTimeMultiplier), b.Farm);
    }
    private static void ValidateOne(double value, string name, FarmKey farm)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new CustomBasicException($"Invalid {name}='{value}' for Farm='{farm}'. Must be finite.");
        }

        if (value <= 0)
        {
            throw new CustomBasicException($"Invalid {name}='{value}' for Farm='{farm}'. Must be > 0.");
        }

        if (value > 1.0)
        {
            throw new CustomBasicException(
                $"Invalid {name}='{value}' for Farm='{farm}'. Must be <= 1.0 (game never slower than base).");
        }
    }


    private static BalanceProfileDocument CreateFarm(
        string playerName,
        string theme,
        string profileId,
        double baseMultiplier,

        // Optional per-system overrides
        double? cropOverride = null,
        double? animalOverride = null,
        double? workshopOverride = null,
        double? treeOverride = null,
        double? worksiteOverride = null)
    {
        var farm = new FarmKey(playerName, theme, profileId);

        return new BalanceProfileDocument
        {
            Farm = farm,

            // IMPORTANT:
            // These are TIME multipliers.
            // FinalDuration = BaseDuration × Multiplier

            CropTimeMultiplier = cropOverride ?? baseMultiplier,
            AnimalTimeMultiplier = animalOverride ?? baseMultiplier,
            WorkshopTimeMultiplier = workshopOverride ?? baseMultiplier,
            TreeTimeMultiplier = treeOverride ?? baseMultiplier,
            WorksiteTimeMultiplier = worksiteOverride ?? baseMultiplier
        };
    }

}
