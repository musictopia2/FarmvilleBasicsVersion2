namespace Phase13QuestsBasedOnLevel.Services.Store;
public interface IStoreUiStateRepository
{
    Task<EnumCatalogCategory> LoadAsync();
    Task SaveAsync(EnumCatalogCategory category);
}