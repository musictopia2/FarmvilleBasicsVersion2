namespace Phase04PrepareForMVP1.ImportClasses;
public static class ImportAnimalRecipeClass
{
    public static async Task ImportAnimalsAsync()
    {
        BasicList<AnimalRecipeDocument> list = [];
        list.AddRange(GetCountryRecipes());
        list.AddRange(GetTropicalRecipes());
        AnimalRecipeDatabase db = new();
        await db.ImportAsync(list);
    }
    private static BasicList<AnimalRecipeDocument> GetCountryRecipes()
    {
        string theme = FarmThemeList.Country;
        BasicList<AnimalRecipeDocument> output = [];
        BasicList<AnimalProductionOption> options = [];
        options.Add(new AnimalProductionOption
        {
            Duration = TimeSpan.FromMinutes(1),
            Input = 3,
            Output = new(CountryItemList.Milk, 1),
            Required = CountryItemList.Wheat
        });
        options.Add(new AnimalProductionOption
        {
            Duration = TimeSpan.FromMinutes(3),
            Input = 9,
            Output = new(CountryItemList.Milk, 3),
            Required = CountryItemList.Wheat
        });
        options.Add(new AnimalProductionOption
        {
            Duration = TimeSpan.FromMinutes(30),
            Input = 24,
            Output = new(CountryItemList.Milk, 8),
            Required = CountryItemList.Wheat
        });
        AnimalRecipeDocument recipe = new()
        {
            Animal = CountryAnimalListClass.Cow,
            Options = options,
            Theme = theme
        };
        output.Add(recipe);
        options = [];
        options.Add(new AnimalProductionOption
        {
            Duration = TimeSpan.FromMinutes(3),
            Input = 2,
            Output = new(CountryItemList.GoatMilk, 2),
            Required = CountryItemList.Carrot
        });
        options.Add(new AnimalProductionOption
        {
            Duration = TimeSpan.FromMinutes(8),
            Input = 4,
            Output = new(CountryItemList.GoatMilk, 4),
            Required = CountryItemList.Carrot
        });
        options.Add(new AnimalProductionOption
        {
            Duration = TimeSpan.FromMinutes(30),
            Input = 8,
            Output = new(CountryItemList.GoatMilk, 8),
            Required = CountryItemList.Carrot
        });
        recipe = new()
        {
            Animal = CountryAnimalListClass.Goat,
            Options = options,
            Theme = theme
        };
        output.Add(recipe);

        options = [];
        options.Add(new AnimalProductionOption
        {
            Duration = TimeSpan.FromMinutes(30),
            Input = 4,
            Output = new(CountryItemList.Wool, 3),
            Required = CountryItemList.Tomato
        });
        options.Add(new AnimalProductionOption
        {
            Duration = TimeSpan.FromHours(1),
            Input = 8,
            Output = new(CountryItemList.Wool, 6),
            Required = CountryItemList.Tomato
        });
        options.Add(new AnimalProductionOption
        {
            Duration = TimeSpan.FromHours(4),
            Input = 12,
            Output = new(CountryItemList.Wool, 12),
            Required = CountryItemList.Tomato
        });
        recipe = new()
        {
            Animal = CountryAnimalListClass.Sheep,
            Options = options,
            Theme = theme
        };
        output.Add(recipe);
        return output;
    }
    
    private static BasicList<AnimalRecipeDocument> GetTropicalRecipes()
    {
        string theme = FarmThemeList.Tropical;
        BasicList<AnimalRecipeDocument> output = [];
        BasicList<AnimalProductionOption> options = [];
        options.Add(new AnimalProductionOption
        {
            Duration = TimeSpan.FromMinutes(1),
            Input = 1,
            Output = new(TropicalItemList.Fish, 2),
            Required = TropicalItemList.Pineapple
        });
        options.Add(new AnimalProductionOption
        {
            Duration = TimeSpan.FromMinutes(2),
            Input = 2,
            Output = new(TropicalItemList.Fish, 4),
            Required = TropicalItemList.Pineapple
        });
        options.Add(new AnimalProductionOption
        {
            Duration = TimeSpan.FromMinutes(30),
            Input = 4,
            Output = new(TropicalItemList.Fish, 8),
            Required = TropicalItemList.Pineapple
        });
        AnimalRecipeDocument recipe = new()
        {
            Animal = TropicalAnimalListClass.Dolphin,
            Options = options,
            Theme = theme
        };
        output.Add(recipe);

        options = [];
        options.Add(new AnimalProductionOption
        {
            Duration = TimeSpan.FromMinutes(2),
            Input = 2,
            Output = new(TropicalItemList.Egg, 1),
            Required = TropicalItemList.Rice
        });
        options.Add(new AnimalProductionOption
        {
            Duration = TimeSpan.FromMinutes(6),
            Input = 6,
            Output = new(TropicalItemList.Egg, 3),
            Required = TropicalItemList.Rice
        });
        options.Add(new AnimalProductionOption
        {
            Duration = TimeSpan.FromMinutes(90),
            Input = 20,
            Output = new(TropicalItemList.Egg, 10),
            Required = TropicalItemList.Rice
        });
        recipe = new()
        {
            Animal = TropicalAnimalListClass.Chicken,
            Options = options,
            Theme = theme
        };
        output.Add(recipe);

        options = [];
        options.Add(new AnimalProductionOption
        {
            Duration = TimeSpan.FromMinutes(3),
            Input = 2,
            Output = new(TropicalItemList.Mushroom, 1),
            Required = TropicalItemList.Tapioca
        });
        options.Add(new AnimalProductionOption
        {
            Duration = TimeSpan.FromMinutes(9),
            Input = 6,
            Output = new(TropicalItemList.Mushroom, 3),
            Required = TropicalItemList.Tapioca
        });
        options.Add(new AnimalProductionOption
        {
            Duration = TimeSpan.FromHours(2),
            Input = 20,
            Output = new(TropicalItemList.Mushroom, 10),
            Required = TropicalItemList.Tapioca
        });
        recipe = new()
        {
            Animal = TropicalAnimalListClass.Boar,
            Options = options,
            Theme = theme
        };
        output.Add(recipe);
        return output;
    }
}