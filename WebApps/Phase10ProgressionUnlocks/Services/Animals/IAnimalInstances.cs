namespace Phase10ProgressionUnlocks.Services.Animals;
public interface IAnimalInstances
{
    Task<BasicList<AnimalAutoResumeModel>> GetAnimalInstancesAsync();
}