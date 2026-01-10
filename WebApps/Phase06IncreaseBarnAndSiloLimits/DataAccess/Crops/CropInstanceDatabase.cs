using Phase06IncreaseBarnAndSiloLimits.Services.Core;

namespace Phase06IncreaseBarnAndSiloLimits.DataAccess.Crops;
// All crops from registry exist and are unlocked.
// Progression rules will be added later.
public class CropInstanceDatabase(FarmKey farm, ICropRegistry recipes) : ListDataAccess<CropInstanceDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, ICropInstances,
    ICropPersistence
{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "CropInstances";
    async Task<CropSystemState> ICropInstances.GetCropInstancesAsync()
    {
        var list = await recipes.GetCropsAsync();
        CropSystemState output = new();
        var others = await GetDocumentsAsync();
        foreach (var item in list)
        {
            CropDataModel crop = new()
            {
                Item = item.Item,
                Unlocked = true
            };
            output.Crops.Add(crop);
        }
        output.Slots = others.Single(x => x.Farm.Equals(farm)).Slots;
        return output;
    }
    async Task ICropPersistence.SaveCropsAsync(BasicList<CropAutoResumeModel> slots)
    {
        var list = await GetDocumentsAsync();
        var item = list.Single(x => x.Farm.Equals(farm));
        item.Slots = slots;
        await UpsertRecordsAsync(list);
    }
}