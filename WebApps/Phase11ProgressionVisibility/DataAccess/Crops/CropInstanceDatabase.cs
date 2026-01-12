namespace Phase11ProgressionVisibility.DataAccess.Crops;
public class CropInstanceDatabase(FarmKey farm) : ListDataAccess<CropInstanceDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, ICropRepository
{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "CropInstances";
    async Task<CropSystemState> ICropRepository.LoadAsync()
    {
        CropSystemState output = new();
        var firsts = await GetDocumentsAsync();
        var document = firsts.GetSingleDocument(farm);
        output.Crops = document.Crops;
        output.Slots = document.Slots;
        return output;
    }

    async Task ICropRepository.SaveAsync(CropSystemState state)
    {
        var list = await GetDocumentsAsync();
        var item = list.GetSingleDocument(farm);

        item.Slots = state.Slots;
        item.Crops = state.Crops;
        await UpsertRecordsAsync(list);

    }
}