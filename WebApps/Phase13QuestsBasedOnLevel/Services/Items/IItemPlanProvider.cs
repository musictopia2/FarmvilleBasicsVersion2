namespace Phase13QuestsBasedOnLevel.Services.Items;
public interface IItemPlanProvider
{
    Task<BasicList<ItemPlanModel>> GetPlanAsync(FarmKey farm);
}