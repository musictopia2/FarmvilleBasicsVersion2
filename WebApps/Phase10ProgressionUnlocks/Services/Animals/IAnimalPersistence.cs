namespace Phase10ProgressionUnlocks.Services.Animals;
public interface IAnimalPersistence
{
    Task SaveAnimalsAsync(BasicList<AnimalAutoResumeModel> animals);
}