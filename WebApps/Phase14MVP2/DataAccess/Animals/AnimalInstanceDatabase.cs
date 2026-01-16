namespace Phase14MVP2.DataAccess.Animals;

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
        BasicList<AnimalAutoResumeModel> output = firsts.GetSingleDocument(farm).Animals;
        return output;
    }


    async Task IAnimalRepository.SaveAsync(BasicList<AnimalAutoResumeModel> animals)
    {
        var list = await GetDocumentsAsync();
        var item = list.GetSingleDocument(farm);
        item.Animals = animals;
        await UpsertRecordsAsync(list);
    }
}