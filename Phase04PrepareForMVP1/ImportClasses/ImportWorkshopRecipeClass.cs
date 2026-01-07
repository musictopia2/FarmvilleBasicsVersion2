namespace Phase04PrepareForMVP1.ImportClasses;

public static class ImportWorkshopRecipeClass
{
    public static async Task ImportWorkshopsAsync()
    {
        BasicList<WorkshopRecipeDocument> list = [];
        list.AddRange(GetCountryRecipes());
        list.AddRange(GetTropicalRecipes());
        list.ForEach(Validate);
        WorkshopRecipeDatabase db = new();
        await db.ImportAsync(list);
    }

    // ------------------------
    // Helper: prevents Item/Output mismatch
    // ------------------------
    private static void AddWorkshopRecipe(
        BasicList<WorkshopRecipeDocument> list,
        string item,
        string buildingName,
        string theme,
        TimeSpan duration,
        Action<Dictionary<string, int>> inputs,
        int outputAmount = 1)
    {
        var dict = new Dictionary<string, int>();
        inputs(dict);

        list.Add(new WorkshopRecipeDocument
        {
            Item = item,
            BuildingName = buildingName,
            Inputs = dict,
            Output = new ItemAmount(item, outputAmount), // ✅ always matches item
            Duration = duration,
            Theme = theme
        });
    }

    // ------------------------
    // Country
    // ------------------------
    private static BasicList<WorkshopRecipeDocument> GetCountryRecipes()
    {
        string theme = FarmThemeList.Country;

        BasicList<WorkshopRecipeDocument> output = [];

        AddWorkshopRecipe(output, CountryItemList.Flour, CountryWorkshopList.Windmill, theme,
            TimeSpan.FromSeconds(30),
            i => { i[CountryItemList.Wheat] = 3; });

        AddWorkshopRecipe(output, CountryItemList.Sugar, CountryWorkshopList.Windmill, theme,
            TimeSpan.FromMinutes(2),
            i => { i[CountryItemList.Corn] = 2; });

        AddWorkshopRecipe(output, CountryItemList.Biscuit, CountryWorkshopList.PastryOven, theme,
            TimeSpan.FromSeconds(30),
            i =>
            {
                i[CountryItemList.Flour] = 1;
                i[CountryItemList.Milk] = 1;
            });

        AddWorkshopRecipe(output, CountryItemList.ApplePie, CountryWorkshopList.PastryOven, theme,
            TimeSpan.FromMinutes(1),
            i =>
            {
                i[CountryItemList.Flour] = 2;
                i[CountryItemList.Apple] = 4;
                i[CountryItemList.Milk] = 1;
            });

        AddWorkshopRecipe(output, CountryItemList.FarmersSoup, CountryWorkshopList.StovetopOven, theme,
            TimeSpan.FromMinutes(3.5),
            i =>
            {
                i[CountryItemList.GoatMilk] = 4;
                i[CountryItemList.Carrot] = 5;
                i[CountryItemList.Tomato] = 2;
            });

        AddWorkshopRecipe(output, CountryItemList.GranolaBar, CountryWorkshopList.StovetopOven, theme,
            TimeSpan.FromMinutes(7),
            i =>
            {
                i[CountryItemList.HoneyComb] = 1;
                i[CountryItemList.Wheat] = 5;
                i[CountryItemList.Strawberry] = 4;
            });

        AddWorkshopRecipe(output, CountryItemList.Trousers, CountryWorkshopList.Loom, theme,
            TimeSpan.FromMinutes(20),
            i => { i[CountryItemList.Wool] = 6; });

        AddWorkshopRecipe(output, CountryItemList.Blanket, CountryWorkshopList.Loom, theme,
            TimeSpan.FromMinutes(10),
            i => { i[CountryItemList.Wool] = 3; });

        return output;
    }


    // ------------------------
    // Tropical
    // ------------------------
    private static BasicList<WorkshopRecipeDocument> GetTropicalRecipes()
    {
        string theme = FarmThemeList.Tropical;

        BasicList<WorkshopRecipeDocument> output = [];

        AddWorkshopRecipe(output, TropicalItemList.PineappleSmoothie, TropicalWorkshopList.HuluHit, theme,
            TimeSpan.FromMinutes(1),
            i => { i[TropicalItemList.Pineapple] = 3; });

        AddWorkshopRecipe(output, TropicalItemList.PinaColada, TropicalWorkshopList.HuluHit, theme,
            TimeSpan.FromMinutes(5),
            i =>
            {
                i[TropicalItemList.Coconut] = 3;
                i[TropicalItemList.Pineapple] = 2;
            });

        AddWorkshopRecipe(output, TropicalItemList.SteamedRice, TropicalWorkshopList.SushiStand, theme,
            TimeSpan.FromSeconds(30),
            i => { i[TropicalItemList.Rice] = 2; });

        AddWorkshopRecipe(output, TropicalItemList.FishRoll, TropicalWorkshopList.SushiStand, theme,
            TimeSpan.FromMinutes(3),
            i =>
            {
                i[TropicalItemList.Fish] = 2;
                i[TropicalItemList.SteamedRice] = 1;
            });

        AddWorkshopRecipe(output, TropicalItemList.GrilledCrab, TropicalWorkshopList.Grill, theme,
            TimeSpan.FromMinutes(3),
            i => { i[TropicalItemList.Crab] = 1; });

        AddWorkshopRecipe(output, TropicalItemList.FriedRice, TropicalWorkshopList.Grill, theme,
            TimeSpan.FromMinutes(8),
            i =>
            {
                i[TropicalItemList.Egg] = 2;
                i[TropicalItemList.SteamedRice] = 1;
                i[TropicalItemList.Pineapple] = 1;
            });

        AddWorkshopRecipe(output, TropicalItemList.Ceviche, TropicalWorkshopList.BeachfrontKitchen, theme,
            TimeSpan.FromMinutes(10),
            i =>
            {
                i[TropicalItemList.Fish] = 3;
                i[TropicalItemList.Coconut] = 2;
                i[TropicalItemList.Lime] = 2;
            });

        AddWorkshopRecipe(output, TropicalItemList.TruffleFries, TropicalWorkshopList.BeachfrontKitchen, theme,
            TimeSpan.FromMinutes(20),
            i =>
            {
                i[TropicalItemList.Tapioca] = 2;
                i[TropicalItemList.Mushroom] = 2;
            });

        return output;
    }


    // ------------------------
    // Validation
    // ------------------------
    public static void Validate(WorkshopRecipeDocument doc)
    {
        if (doc.Item != doc.Output.Item)
        {
            throw new CustomBasicException(
                $"Workshop recipe mismatch: Item='{doc.Item}' Output='{doc.Output.Item}' Building='{doc.BuildingName}' Theme='{doc.Theme}'");
        }

        if (doc.Inputs.Count == 0)
        {
            throw new CustomBasicException(
                $"Workshop recipe has no inputs: Item='{doc.Item}' Building='{doc.BuildingName}'");
        }

        if (doc.Output.Amount <= 0)
        {
            throw new CustomBasicException(
                $"Workshop recipe output amount invalid: Item='{doc.Item}' Amount='{doc.Output.Amount}'");
        }

        if (string.IsNullOrWhiteSpace(doc.BuildingName))
        {
            throw new CustomBasicException(
                $"Workshop recipe missing building name: Item='{doc.Item}'");
        }
    }
}
