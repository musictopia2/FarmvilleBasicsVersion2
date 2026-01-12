namespace Phase10ProgressionUnlocks.DataAccess.Crops;
public class CropInstanceDatabase(FarmKey farm) : ListDataAccess<CropInstanceDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, ICropInstances,
    ICropPersistence
{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "CropInstances";
    async Task<CropSystemState> ICropInstances.GetCropInstancesAsync()
    {
        CropSystemState output = new();
        var firsts = await GetDocumentsAsync();
        var document = firsts.GetSingleDocument(farm);
        output.Crops = document.Crops;
        output.Slots = document.Slots;
        return output;
    }
    async Task ICropPersistence.SaveCropsAsync(BasicList<CropAutoResumeModel> slots)
    {
        var list = await GetDocumentsAsync();
        var item = list.GetSingleDocument(farm);
        item.Slots = slots;
        await UpsertRecordsAsync(list);
    }
}