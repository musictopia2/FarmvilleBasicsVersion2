namespace Phase04PrepareForMVP1.ImportClasses;
internal static class ImportWorkerRecipeClass
{
    public static async Task ImportWorkersAsync()
    {
        BasicList<WorkerRecipeDocument> list = [];
        list.AddRange(GetCountry());
        list.AddRange(GetTropical());
        WorkerRecipeDatabase db = new();
        await db.ImportAsync(list);
    }
    private static BasicList<WorkerRecipeDocument> GetCountry()
    {
        BasicList<WorkerRecipeDocument> output = [];
        string theme = FarmThemeList.Country;
        WorkerRecipeDocument worker;
        WorkerBenefit benefit;
        worker = new()
        {
            WorkerName = CountryWorkerListClass.Bob,
            Details = "Increased Chance of Chives at Grandma's Glade",
            Theme = theme
        };
        benefit = new()
        {
            Worksite = CountryWorksiteListClass.GrandmasGlade,
            Item = CountryItemList.Chives,
            ChanceModifier = 0.3
            //QuantityBonus = 1 //i think is needed
        };
        worker.Benefits.Add(benefit);


        //try without the bonus (if nothing is given and is not a normal benefit, then needs to be 1).
        output.Add(worker);
        worker = new()
        {
            WorkerName = CountryWorkerListClass.Alice,
            Details = "Gets 1 strawberry at Grandma's Glade.",
            Theme = theme

        };
        benefit = new()
        {
            Worksite = CountryWorksiteListClass.GrandmasGlade,
            Item = CountryItemList.Strawberry,
            Guarantee = true
            
        };
        worker.Benefits.Add(benefit);

        output.Add(worker);
        worker = new()
        {
            WorkerName = CountryWorkerListClass.Clara,
            Details = "Gets 1 carrot from the pond.",
            Theme = theme
        };
        benefit = new()
        {
            Worksite = CountryWorksiteListClass.Pond,
            Item = CountryItemList.Carrot,
            Guarantee = true
        };
        worker.Benefits.Add(benefit);
        output.Add(worker);
        
        worker = new()
        {
            WorkerName = CountryWorkerListClass.Daniel,
            Details = "General Worker",
            //Details = "Gets 1 extra cooper from the mines and is a guarantee.",
            Theme = theme
        };
        //benefit = new()
        //{
        //    Worksite = CountryWorksiteListClass.Mines,
        //    Item = CountryItemList.Cooper,
        //    GivesExtra = true,
        //    Guarantee = true
        //};
        //worker.Benefits.Add(benefit);
        output.Add(worker);

        worker = new()
        {
            WorkerName = CountryWorkerListClass.Emma,
            Details = "General Worker",
            //Details = "Gets 1 extra tin from the mines and is a guarantee.",
            Theme = theme
        };
        //benefit = new()
        //{
        //    Worksite = CountryWorksiteListClass.Mines,
        //    Item = CountryItemList.Tin,
        //    GivesExtra = true,
        //    Guarantee = true
        //};
        //worker.Benefits.Add(benefit);
        output.Add(worker);


        return output;
    }
    private static BasicList<WorkerRecipeDocument> GetTropical()
    {
        BasicList<WorkerRecipeDocument> output = [];
        string theme = FarmThemeList.Tropical;
        string general = "General Worker";




        WorkerRecipeDocument worker;

        BasicList<string> list =
            [
                TropicalWorkerListClass.George,
                TropicalWorkerListClass.Ethan,
                TropicalWorkerListClass.Fiona,
                TropicalWorkerListClass.Toby
            ];

        foreach (var item in list)
        {
            worker = new()
            {
                Theme = theme,
                Details = general,
                Benefits = [],
                WorkerName = item
            };
            output.Add(worker);
        }
        return output;
    }
}