namespace Phase01IntroduceBarnAndSilo.Services.Animals;
public interface IAnimalInstances
{
    Task<BasicList<AnimalAutoResumeModel>> GetAnimalInstancesAsync();
}