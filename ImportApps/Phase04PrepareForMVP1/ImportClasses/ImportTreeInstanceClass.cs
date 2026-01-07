namespace Phase04PrepareForMVP1.ImportClasses;
public static class ImportTreeInstanceClass
{
    private static BasicList<TreeRecipeDocument> _recipes = [];

    // MVP1 rule: each farm starts with 1 instance of each tree type in its theme.
    // Future profiles/modes may choose different counts.
    private const int _productionTreesPerRecipe = 1;

    public static async Task ImportTreesAsync()
    {
        // Load recipes once so we can generate instances per theme
        TreeRecipeDatabase recipeDb = new();
        _recipes = await recipeDb.GetRecipesAsync();

        if (_recipes.Count == 0)
        {
            throw new CustomBasicException("No tree recipes were imported.");
        }

        BasicList<TreeInstanceDocument> list = [];

        // Production farms for MVP1
        list.Add(CreateProduction(PlayerList.Andy, FarmThemeList.Country));
        list.Add(CreateProduction(PlayerList.Cristina, FarmThemeList.Country));
        list.Add(CreateProduction(PlayerList.Andy, FarmThemeList.Tropical));
        list.Add(CreateProduction(PlayerList.Cristina, FarmThemeList.Tropical));

        // Future: other profiles/modes
        // list.Add(CreateFarm(PlayerList.Andy, FarmThemeList.Country, ProfileIdList.Test, treesPerRecipe: 3));

        TreeInstanceDatabase db = new();
        await db.ImportAsync(list);
    }

    private static TreeInstanceDocument CreateProduction(string playerName, string theme)
        => CreateFarm(playerName, theme, ProfileIdList.Production, _productionTreesPerRecipe);

    private static TreeInstanceDocument CreateFarm(string playerName, string theme, string profileId, int treesPerRecipe)
    {
        // If FarmKey is positional record struct: new FarmKey(playerName, theme, profileId)
        // If FarmKey is init-properties type: use object initializer (below)
        var farm = new FarmKey
        {
            PlayerName = playerName,
            Theme = theme,
            ProfileId = profileId
        };

        return CreateInstance(farm, treesPerRecipe);
    }

    private static TreeInstanceDocument CreateInstance(FarmKey farm, int treesPerRecipe)
    {
        BasicList<TreeAutoResumeModel> trees = [];

        // Only recipes for this farm’s theme (and optionally profile, if your recipes are profile-specific)
        var recipesForTheme = _recipes
            .Where(r => r.Theme == farm.Theme /* && r.Mode == farm.ProfileId */)
            .ToBasicList();

        if (recipesForTheme.Count == 0)
        {
            throw new CustomBasicException($"No tree recipes found for theme '{farm.Theme}'.");
        }

        recipesForTheme.ForEach(recipe =>
        {
            treesPerRecipe.Times(_ =>
            {
                trees.Add(new TreeAutoResumeModel
                {
                    TreeName = recipe.TreeName
                });
            });
        });

        return new TreeInstanceDocument
        {
            Farm = farm,
            Trees = trees
        };
    }

}