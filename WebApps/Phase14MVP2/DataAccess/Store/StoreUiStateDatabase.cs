namespace Phase14MVP2.DataAccess.Store;
public class StoreUiStateDatabase(FarmKey farm) : ListDataAccess<StoreUiStateDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IStoreUiStateRepository
{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "StoreUiState";
    async Task<EnumCatalogCategory> IStoreUiStateRepository.LoadAsync()
    {
        var list = await GetDocumentsAsync();
        return list.GetSingleDocument(farm).LastCategory;
    }
    async Task IStoreUiStateRepository.SaveAsync(EnumCatalogCategory category)
    {
        var list = await GetDocumentsAsync();
        list.GetSingleDocument(farm).LastCategory = category;
        await UpsertRecordsAsync(list);
    }
}