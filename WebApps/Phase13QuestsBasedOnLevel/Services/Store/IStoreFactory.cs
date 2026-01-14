namespace Phase13QuestsBasedOnLevel.Services.Store;
public interface IStoreFactory
{
    StoreServicesContext GetStoreServices(FarmKey farm);
}