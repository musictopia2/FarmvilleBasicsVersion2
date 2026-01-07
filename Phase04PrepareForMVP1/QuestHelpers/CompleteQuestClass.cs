namespace Phase04PrepareForMVP1.QuestHelpers;
internal static class CompleteQuestClass
{

    static ItemSourceCatalog _catalog = new();
    static BasicList<WorkshopRecipeDocument> _workshopRecipes = [];


    public static async Task<BasicList<QuestRecipe>> GetQuestsAsync(FarmKey farm)
    {
        _catalog = new();
        await AddTreesAsync(farm);
        await AddCropsAsync(farm);
        await AddAnimalsAsync(farm);
        await AddWorkshopsAsync(farm);
        await AddWorksitesAsync(farm);
        var firsts = AnalyzeWorkshops(farm);

        var seconds = AnalyzeWorksitesFromCatalog();

        var thirds = AnalyzeBasics();

        QuestContainer container = new()
        {
            Basics = thirds,
            Workshops = firsts,
            Worksites = seconds
        };

        container.Worksites.ForEach(item =>
        {
            int amount = WorksiteRequirementPicker.PickRequiredAmount(item);
        });





        container.Workshops.ForEach(item =>
        {
            int amount = RequirementPicker.ForWorkshop(item);
        });


        container.Basics.ForEach(item =>
        {
            int amount;
            if (item.Source == EnumItemSource.Crop)
            {
                amount = RequirementPicker.ForCrop(item.BaseTime);
            }
            else if (item.Source == EnumItemSource.Tree)
            {
                amount = RequirementPicker.ForTree(item.BaseTime, item.Amount);
            }
            else if (item.Source == EnumItemSource.Animal)
            {
                amount = RequirementPicker.ForAnimal(item.BaseTime, item.Amount);
            }
            else
            {
                throw new CustomBasicException("Nothing found");
            }
            //Console.WriteLine($"Required Was {amount} for {item.Item}");
        });

        //based on the rules, 40 was the most amount of quests that can be achieved.
        QuestGenerationSettings settings = new()
        {
            TotalQuests = 20,
            Weighting = new QuestGenerationSettings.Weights { Basics = 40, Workshops = 60, Worksites = 10 },
            Limits = new QuestGenerationSettings.Caps
            {
                MaxWorksiteQuests = 3,
                MaxRareWorksiteQuests = 1,
                MaxMultiStepWorkshopQuests = 3, //biscuits are multistep
                MaxRareInputWorkshopQuests = 10,
                MaxSameBasicItemQuests = 2,

                MaxSameWorkshopItemQuests = 2 // NEW: allow up to 2 per pack for same workshop output
            },
            AllowWorkshopRepeatsInPack = true,
            WorkshopRepeatKeyIsOutputItem = true, // repeats counted by OutputItem like "Biscuits"
            Duplicates = new QuestGenerationSettings.DuplicatePolicy
            {
                WorkshopRepeatWeight1 = 0.70,
                WorkshopRepeatWeight2Plus = 0.40
            }
        };

        QuestPackGenerator gen = new();
        BasicList<QuestRecipe> quests = gen.Generate(container, settings);
        //foreach (var item in quests)
        //{
        //    Console.WriteLine($"{item.Item} needs {item.Required}");
            
        
        //}
        //Console.ReadLine();
        return quests;
        

    }


    private static BasicList<BasicItem> AnalyzeBasics()
    {
        BasicList<BasicItem> output = [];
        _catalog.Items.ForConditionalItems(profile => profile.Has(EnumItemSource.Crop) ||
            profile.Has(EnumItemSource.Tree) ||
            profile.Has(EnumItemSource.Animal)

        , profile =>
        {
            BasicItem item = new()
            {
                Amount = profile.Amount,
                BaseTime = profile.Time,
                Item = profile.ItemName,
                Source = profile.CurrentSource
            };
            output.Add(item);
        });
        return output;
    }

    private static BasicList<WorksiteOutputAnalysis> AnalyzeWorksitesFromCatalog()
    {
        BasicList<WorksiteOutputAnalysis> analyses = [];


        _catalog.Items.ForConditionalItems(profile => profile.Has(EnumItemSource.Worksite), profile =>
        {
            foreach (var a in profile.WorksiteAvailabilities)
            {

                ItemSourceProfile other = _catalog.Get(profile.ItemName);
                int count = _workshopRecipes
                    .SelectMany(r => r.Inputs)
                    .Count(i => i.Key == profile.ItemName);

                analyses.Add(new WorksiteOutputAnalysis
                {
                    Location = a.Location,
                    Item = profile.ItemName,
                    Rarity = a.Rarity,
                    OutputAmount = profile.Amount,
                    Duration = profile.Time,
                    FromWorkshops = count
                });
            }
        });


        return analyses;
    }


    private static BasicList<WorkshopOutputAnalysis> AnalyzeWorkshops(FarmKey farm)
    {
        BasicList<WorkshopOutputAnalysis> analyses = [];

        _workshopRecipes.ForConditionalItems(x => x.Theme == farm.Theme, recipe =>
        {
            var a = new WorkshopOutputAnalysis
            {
                WorkshopName = recipe.BuildingName,
                OutputItem = recipe.Output.Item,
                OutputAmount = recipe.Output.Amount,
                CraftTime = recipe.Duration
            };

            TimeSpan cropPrepMax = TimeSpan.Zero;   // grouped crops
            TimeSpan otherPrepSum = TimeSpan.Zero;  // serial inputs

            foreach (var input in recipe.Inputs)
            {
                string item = input.Key;
                int qtyNeeded = input.Value;

                var profile = _catalog.Get(item);

                int yield = Math.Max(1, profile.Amount);
                int runs = (int)Math.Ceiling(qtyNeeded / (double)yield);

                TimeSpan ingredientPrep;

                // ---- PARALLEL: crops (grouped by max) ----
                if (profile.CurrentSource == EnumItemSource.Crop)
                {
                    ingredientPrep = profile.Time; // one crop cycle

                    if (ingredientPrep > cropPrepMax)
                    {
                        cropPrepMax = ingredientPrep;
                    }
                }
                // ---- SERIAL: everything else ----
                else
                {
                    ingredientPrep = TimeSpan.FromTicks(profile.Time.Ticks * runs);
                    otherPrepSum += ingredientPrep;
                }

                // ---- dependency / rarity logic (unchanged) ----
                if (profile.Has(EnumItemSource.Worksite))
                {
                    a.WorksiteInputs++;

                    var worst = profile.WorstWorksiteRarity();
                    if (WorstRank(worst) > WorstRank(a.WorstWorksiteInputRarity))
                    {
                        a.WorstWorksiteInputRarity = worst;
                    }
                }

                if (profile.Has(EnumItemSource.Workshop))
                {
                    a.WorkshopInputs++;
                }
                else if (profile.Has(EnumItemSource.Crop))
                {
                    a.CropInputs++;
                }
                else if (profile.Has(EnumItemSource.Tree))
                {
                    a.TreeInputs++;
                }
                else if (profile.Has(EnumItemSource.Animal))
                {
                    a.AnimalInputs++;
                }
            }
            a.InputPrep = otherPrepSum + otherPrepSum;
            

            analyses.Add(a);
        });

        return analyses;
    }
    private static int WorstRank(EnumWorksiteRarity rarity) => rarity switch
    {
        EnumWorksiteRarity.None => 0,
        EnumWorksiteRarity.Guaranteed => 1,
        EnumWorksiteRarity.Common => 2,
        EnumWorksiteRarity.Rare => 3,
        _ => 0
    };


    private static async Task AddTreesAsync(FarmKey farm)
    {
        TreeRecipeDatabase db = new();
        BasicList<TreeRecipeDocument> recipes = await db.GetRecipesAsync();
        recipes.ForConditionalItems(x => x.Theme == farm.Theme, recipe =>
        {

            _catalog.GetOrCreate(recipe.Item, recipe.ProductionTimeForEach, 1).AddSource(EnumItemSource.Tree);

        });
    }

    private static async Task AddCropsAsync(FarmKey farm)
    {
        CropRecipeDatabase db = new();
        BasicList<CropRecipeDocument> recipes = await db.GetRecipesAsync();
        //BasicList<QuestCandidate> output = [];
        recipes.ForConditionalItems(x => x.Theme == farm.Theme, recipe =>
        {
            _catalog.GetOrCreate(recipe.Item, recipe.Duration, 2).AddSource(EnumItemSource.Crop);

        });
    }

    private static async Task AddAnimalsAsync(FarmKey farm)
    {
        AnimalRecipeDatabase db = new();
        BasicList<AnimalRecipeDocument> recipes = await db.GetRecipesAsync();
        recipes.ForConditionalItems(x => x.Theme == farm.Theme, recipe =>
        {
            _catalog.GetOrCreate(recipe.Options.First().Output.Item,
                recipe.Options.First().Duration,
                recipe.Options.First().Output.Amount)
                .AddAnimal(recipe.Options.First().Required);
        });
    }

    private static async Task AddWorkshopsAsync(FarmKey farm)
    {
        WorkshopRecipeDatabase db = new();
        _workshopRecipes = await db.GetRecipesAsync();
        _workshopRecipes.ForConditionalItems(x => x.Theme == farm.Theme, recipe =>
        {
            _catalog.GetOrCreate(recipe.Output.Item, recipe.Duration, 1).AddSource(EnumItemSource.Workshop);

        });
    }
    private static async Task AddWorksitesAsync(FarmKey farm)
    {
        WorksiteRecipeDatabase db = new();
        BasicList<WorksiteRecipeDocument> recipes = await db.GetRecipesAsync();
        recipes.ForConditionalItems(x => x.Theme == farm.Theme, recipe =>
        {
            recipe.BaselineBenefits.ForConditionalItems(x => x.Optional == false, benefit =>
            {

                //iffy because needs to capture if its rare or not.


                EnumWorksiteRarity category;
                int quantity = benefit.Quantity;

                if (benefit.Guarantee && benefit.EachWorkerGivesOne)
                {
                    category = EnumWorksiteRarity.Guaranteed;
                    quantity = benefit.Quantity * recipe.MaximumWorkers;
                }
                else if (benefit.Guarantee)
                {
                    category = EnumWorksiteRarity.Guaranteed;
                }
                else if (benefit.Chance > .25)
                {
                    category = EnumWorksiteRarity.Common;
                }
                else
                {
                    category = EnumWorksiteRarity.Rare;
                }
                WorksiteAvailability available = new()
                {
                    Rarity = category,
                    Location = recipe.Location
                };
                _catalog.GetOrCreate(benefit.Item, recipe.Duration, benefit.Quantity).AddWorksiteAvailability(available);

            });
        });
    }

}
