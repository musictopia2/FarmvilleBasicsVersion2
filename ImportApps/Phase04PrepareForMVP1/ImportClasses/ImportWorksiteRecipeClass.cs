namespace Phase04PrepareForMVP1.ImportClasses;
public static class ImportWorksiteRecipeClass
{
    public static async Task ImportWorksitesAsync()
    {
        BasicList<WorksiteRecipeDocument> list = [];
        list.AddRange(GetCountryRecipes());
        list.AddRange(GetTropicalRecipes());
        WorksiteRecipeDatabase db = new();
        await db.ImportAsync(list);
    }
    private static BasicList<WorksiteRecipeDocument> GetCountryRecipes()
    {
        string theme = FarmThemeList.Country;
        BasicList<WorksiteRecipeDocument> output = [];
        WorksiteRecipeDocument recipe = new()
        {
            StartText = "Go Foraging!",
            Location = CountryWorksiteListClass.GrandmasGlade,
            Duration = TimeSpan.FromMinutes(15),
            MaximumWorkers = 2,
            Theme = theme,
            CanFocus = false //no need from here since does not take long.  plus no rare items.
            
        };
        recipe.Inputs.Add(CountryItemList.Biscuit, 1);
        WorksiteBaselineBenefit benefit = new()
        {
            Guarantee = true,
            Item = CountryItemList.Blackberry
        };
        recipe.BaselineBenefits.Add(benefit);
        benefit = new()
        {
            Chance = 0.4,
            Item = CountryItemList.Chives
        };
        recipe.BaselineBenefits.Add(benefit);
        //benefit = new()
        //{
        //    Chance = 0.03,
        //    Item = CountryItemList.BarnNail
        //};
        //recipe.BaselineBenefits.Add(benefit);
        output.Add(recipe);
        recipe = new()
        {
            StartText = "Go Fishing!",
            Location = CountryWorksiteListClass.Pond,
            Duration = TimeSpan.FromHours(8),
            MaximumWorkers = 4,
            Theme = theme
        };
        recipe.Inputs.Add(CountryItemList.Biscuit, 1);
        recipe.Inputs.Add(CountryItemList.FarmersSoup, 1);

        benefit = new()
        {
            Guarantee = true,
            Item = CountryItemList.Trout
        };
        recipe.BaselineBenefits.Add(benefit);
        //benefit = new()
        //{
        //    Item = CountryItemList.Padlocks,
        //    Chance = 0.03
        //};
        //recipe.BaselineBenefits.Add(benefit);
        //benefit = new()
        //{
        //    Item = CountryItemList.BarnNail,
        //    Chance = 0.06
        //};
        //recipe.BaselineBenefits.Add(benefit);
        benefit = new()
        {
            Item = CountryItemList.Bass,
            Chance = 0.11
        };
        recipe.BaselineBenefits.Add(benefit);
        benefit = new()
        {
            Item = CountryItemList.Mint,
            Chance = 0.13
        };
        recipe.BaselineBenefits.Add(benefit);
        output.Add(recipe);
        recipe = new()
        {
            StartText = "Go Exploring!",
            Location = CountryWorksiteListClass.Mines,
            Duration = TimeSpan.FromHours(2),
            MaximumWorkers = 6,
            Theme = theme
        };
        recipe.Inputs.Add(CountryItemList.GranolaBar, 2);
        recipe.Inputs.Add(CountryItemList.Blanket, 2);

        benefit = new()
        {
            Guarantee = true,
            Item = CountryItemList.Quartz,
            Quantity = 2
        };
        recipe.BaselineBenefits.Add(benefit);
        //benefit = new()
        //{
        //    Item = CountryItemList.Padlocks,
        //    Chance = 0.02
        //};
        //recipe.BaselineBenefits.Add(benefit);
        //benefit = new()
        //{
        //    Item = CountryItemList.BarnNail,
        //    Chance = 0.03,
        //    Optional = true
        //};
        //recipe.BaselineBenefits.Add(benefit);
        benefit = new()
        {
            Item = CountryItemList.Tin,
            Chance = 0.5
        };
        recipe.BaselineBenefits.Add(benefit);
        benefit = new()
        {
            Item = CountryItemList.Cooper,
            Chance = 0.07
        };
        recipe.BaselineBenefits.Add(benefit);
        output.Add(recipe);

        return output;
    }
    private static BasicList<WorksiteRecipeDocument> GetTropicalRecipes()
    {
        string theme = FarmThemeList.Tropical;
        BasicList<WorksiteRecipeDocument> output = [];
        WorksiteRecipeDocument recipe = new()
        {
            StartText = "Go snorkeling!",
            Location = TropicalWorksiteListClass.CorelReef,
            Duration = TimeSpan.FromMinutes(15),
            MaximumWorkers = 3,
            Theme = theme,
            CanFocus = false
        };
        recipe.Inputs.Add(TropicalItemList.PineappleSmoothie, 2);
        WorksiteBaselineBenefit benefit = new()
        {
            Guarantee = true,
            Item = TropicalItemList.Crab,
            EachWorkerGivesOne = true
        };
        recipe.BaselineBenefits.Add(benefit);
        benefit = new()
        {
            Chance = 0.25,
            Item = TropicalItemList.Seashell
        };
        recipe.BaselineBenefits.Add(benefit);
        //later will figure out something else you get from the corel reef

        output.Add(recipe);
        recipe = new()
        {
            StartText = "Take a hot soak!",
            Location = TropicalWorksiteListClass.HotSprings,
            Duration = TimeSpan.FromHours(2),
            MaximumWorkers = 4,
            Theme = theme
        };
        recipe.Inputs.Add(TropicalItemList.Ceviche, 2);

        benefit = new()
        {
            Guarantee = true,
            Item = TropicalItemList.Clay,
            EachWorkerGivesOne = true
        };
        recipe.BaselineBenefits.Add(benefit);


        benefit = new()
        {
            Item = TropicalItemList.Vanilla,
            Chance = 0.13
        };
        recipe.BaselineBenefits.Add(benefit);
        benefit = new()
        {
            Item = TropicalItemList.Jasmine,
            Chance = 0.3
        };
        recipe.BaselineBenefits.Add(benefit);
        output.Add(recipe);

        recipe = new()
        {
            StartText = "Go spelunking!",
            Location = TropicalWorksiteListClass.SmugglersCave,
            Duration = TimeSpan.FromHours(1),
            MaximumWorkers = 4,
            Theme = theme
        };
        recipe.Inputs.Add(TropicalItemList.TruffleFries, 2);
        recipe.Inputs.Add(TropicalItemList.FriedRice, 2);
        benefit = new()
        {
            Guarantee = true,
            Item = TropicalItemList.IronOre,
            EachWorkerGivesOne = true
        };
        recipe.BaselineBenefits.Add(benefit);


        benefit = new()
        {
            Item = TropicalItemList.BlueCrystal,
            Chance = 0.13
        };
        recipe.BaselineBenefits.Add(benefit);
        benefit = new()
        {
            Item = TropicalItemList.SilverOre,
            Chance = 0.3
        };
        recipe.BaselineBenefits.Add(benefit);
        output.Add(recipe);

        return output;

    }
    
}
