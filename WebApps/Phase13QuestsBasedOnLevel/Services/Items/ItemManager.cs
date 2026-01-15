namespace Phase13QuestsBasedOnLevel.Services.Items;
public class ItemManager
{
    private BasicList<ItemPlanModel> _items = [];
    public async Task SetItemStyleContextAsync(ItemServicesContext context, FarmKey farm)
    {
        _items = await context.ItemPlanProvider.GetPlanAsync(farm);
    }
    public BasicList<ItemPlanModel> GetEligibleItems(int level) => _items.Where(x => x.MinLevel <= level).ToBasicList();
}