namespace Phase07EarnCoinFromQuests.Services.Animals;
public interface IAnimalInstances
{
    Task<BasicList<AnimalAutoResumeModel>> GetAnimalInstancesAsync();
}