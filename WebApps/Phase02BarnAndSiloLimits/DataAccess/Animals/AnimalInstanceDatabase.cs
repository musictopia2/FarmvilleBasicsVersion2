using Phase02BarnAndSiloLimits.Services.Core;

namespace Phase02BarnAndSiloLimits.DataAccess.Animals;
public class AnimalInstanceDatabase(FarmKey farm
    ) : ListDataAccess<AnimalInstanceDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IAnimalInstances, IAnimalPersistence

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "AnimalInstances";
    async Task<BasicList<AnimalAutoResumeModel>> IAnimalInstances.GetAnimalInstancesAsync()
    {
        var firsts = await GetDocumentsAsync();
        BasicList<AnimalAutoResumeModel> output = firsts.Single(x => x.Farm.Equals(farm)).Animals;
        return output;
    }
    async Task IAnimalPersistence.SaveAnimalsAsync(BasicList<AnimalAutoResumeModel> animals)
    {
        var list = await GetDocumentsAsync();
        var item = list.Single(x => x.Farm.Equals(farm));
        item.Animals = animals;
        await UpsertRecordsAsync(list);
    }
}