namespace Phase04PrepareForMVP1.ImportClasses;
public static class ImportCropRecipeClass
{
    public static async Task ImportCropsAsync()
    {
        BasicList<CropRecipeDocument> list = [];
        list.AddRange(GetCountryRecipes());
        list.AddRange(GetTropicalRecipes());
        CropRecipeDatabase db = new();
        await db.ImportAsync(list);
    }
    private static BasicList<CropRecipeDocument> GetCountryRecipes()
    {
        string theme = FarmThemeList.Country;
        BasicList<CropRecipeDocument> output = [];
        output.Add(new CropRecipeDocument
        {
            Item = CountryItemList.Wheat,
            Duration = TimeSpan.FromSeconds(30),
            Theme = theme
        });
        output.Add(new CropRecipeDocument
        {
            Item = CountryItemList.Corn,
            Duration = TimeSpan.FromMinutes(2),
            Theme = theme
        });
        output.Add(new CropRecipeDocument
        {
            Item = CountryItemList.HoneyComb,
            Duration = TimeSpan.FromMinutes(45),
            Theme = theme
        });
        output.Add(new CropRecipeDocument
        {
            Item = CountryItemList.Strawberry,
            Duration = TimeSpan.FromHours(1),
            Theme = theme
        });
        output.Add(new CropRecipeDocument
        {
            Item = CountryItemList.Tomato,
            Duration = TimeSpan.FromMinutes(10),
            Theme = theme
        });
        output.Add(new CropRecipeDocument
        {
            Item = CountryItemList.Carrot,
            Duration = TimeSpan.FromMinutes(4),
            Theme = theme
        });
        return output;
    }
    private static BasicList<CropRecipeDocument> GetTropicalRecipes()
    {
        string theme = FarmThemeList.Tropical;
        BasicList<CropRecipeDocument> output = [];
        output.Add(new CropRecipeDocument
        {
            Item = TropicalItemList.Pineapple,
            Duration = TimeSpan.FromSeconds(45),
            Theme = theme
        });
        output.Add(new CropRecipeDocument
        {
            Item = TropicalItemList.Rice,
            Duration = TimeSpan.FromMinutes(1),
            Theme = theme
        });
        output.Add(new CropRecipeDocument
        {
            Item = TropicalItemList.Tapioca,
            Duration = TimeSpan.FromMinutes(20),
            Theme = theme
        });
        return output;

    }
}