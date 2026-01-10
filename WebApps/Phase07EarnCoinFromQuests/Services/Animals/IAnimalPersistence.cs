namespace Phase07EarnCoinFromQuests.Services.Animals;
public interface IAnimalPersistence
{
    Task SaveAnimalsAsync(BasicList<AnimalAutoResumeModel> animals);
}