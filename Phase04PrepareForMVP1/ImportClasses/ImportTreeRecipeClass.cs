namespace Phase04PrepareForMVP1.ImportClasses;
public static class ImportTreeRecipeClass
{
    public static async Task ImportTreesAsync()
    {
        BasicList<TreeRecipeDocument> list = [];
        list.AddRange(GetCountryRecipes());
        list.AddRange(GetTropicalRecipes());
        TreeRecipeDatabase db = new();
        await db.ImportAsync(list);
    }
    private static BasicList<TreeRecipeDocument> GetCountryRecipes()
    {
        string theme = FarmThemeList.Country;
        BasicList<TreeRecipeDocument> output = [];
        TreeRecipeDocument tree = new()
        {
            TreeName = CountryTreeListClass.Apple,
            Item = CountryItemList.Apple,
            ProductionTimeForEach = TimeSpan.FromSeconds(10),
            Theme = theme
        };
        output.Add(tree);
        tree = new()
        {
            TreeName = CountryTreeListClass.Peach,
            Item = CountryItemList.Peach,
            ProductionTimeForEach = TimeSpan.FromHours(1),
            Theme = theme
        };
        output.Add(tree);
        return output;
    }
    private static BasicList<TreeRecipeDocument> GetTropicalRecipes()
    {
        string theme = FarmThemeList.Tropical;
        BasicList<TreeRecipeDocument> output = [];
        TreeRecipeDocument tree = new()
        {
            TreeName = TropicalTreeListClass.Coconut,
            Item = TropicalItemList.Coconut,
            ProductionTimeForEach = TimeSpan.FromMinutes(2),
            Theme = theme
        };
        output.Add(tree);
        tree = new()
        {
            TreeName = TropicalTreeListClass.Lime,
            Item = TropicalItemList.Lime,
            ProductionTimeForEach = TimeSpan.FromMinutes(45),
            Theme = theme
        };
        output.Add(tree);
        return output;
    }
}