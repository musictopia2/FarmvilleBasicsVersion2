namespace Phase12StoreWithBasicPurchases.Services.Workshops;
public class WorkshopInstance
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public int SelectedRecipeIndex { get; set; } = 0;
    public BasicList<UnlockModel> SupportedItems { get; set; } = [];
    public int Capacity { get; set; } = 2; //for now, always 2.  later will rethink.
    public bool Unlocked { get; set; }
    public BasicList<CraftingJobInstance> Queue { get; } = [];
    required public string BuildingName { get; init; }
    public bool CanAccept(WorkshopRecipe recipe)
    {
        if (recipe.BuildingName != BuildingName)
        {
            return false;
        }
        if (Queue.Count >= Capacity)
        {
            return false;
        }
        return true;
    }
    public void Start()
    {
        var next = Queue.First(j => j.State == EnumWorkshopState.Waiting);
        next.Start();
    }
    public void Load(WorkshopAutoResumeModel workshop, BasicList<WorkshopRecipe> recipes, double multiplier)
    {
        Capacity = workshop.Capacity;
        SupportedItems = workshop.SupportedItems;
        if (SupportedItems.Count == 0)
        {
            throw new CustomBasicException("Must support at least one item.");
        }
        Id = workshop.Id;
        Unlocked = workshop.Unlocked;
        SelectedRecipeIndex = workshop.SelectedRecipeIndex;
        Queue.Clear();
        foreach (var item in workshop.Queue)
        {
            WorkshopRecipe recipe = recipes.Single(x => x.Item == item.RecipeItem);
            CraftingJobInstance job = new(recipe, multiplier);
            job.Load(item);
            Queue.Add(job);
        }
    }
    public WorkshopAutoResumeModel GetWorkshopForSaving
    {
        get
        {
            return new()
            {
                Capacity = Capacity,
                Name = BuildingName,
                Unlocked = Unlocked,
                SupportedItems = SupportedItems,
                Queue = Queue.Select(x => x.GetCraftingForSaving).ToBasicList(),
                SelectedRecipeIndex = SelectedRecipeIndex
            };
        }
    }
}