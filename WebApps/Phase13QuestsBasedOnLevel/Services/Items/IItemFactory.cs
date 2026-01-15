namespace Phase13QuestsBasedOnLevel.Services.Items;
public interface IItemFactory
{
    ItemServicesContext GetItemServices(FarmKey farm);
}