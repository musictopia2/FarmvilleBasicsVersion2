namespace Phase10ProgressionUnlocks.DataAccess.Animals;

public class AnimalInstanceDatabase(FarmKey farm
    ) : ListDataAccess<AnimalInstanceDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IAnimalRepository

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "AnimalInstances";
    async Task<BasicList<AnimalAutoResumeModel>> IAnimalRepository.LoadAsync()
    {
        var firsts = await GetDocumentsAsync();
        BasicList<AnimalAutoResumeModel> output = firsts.Single(x => x.Farm.Equals(farm)).Animals;
        return output;
    }


    async Task IAnimalRepository.SaveAsync(BasicList<AnimalAutoResumeModel> animals)
    {
        var list = await GetDocumentsAsync();
        var item = list.Single(x => x.Farm.Equals(farm));
        item.Animals = animals;
        await UpsertRecordsAsync(list);
    }
}