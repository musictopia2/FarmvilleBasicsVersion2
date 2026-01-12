namespace Phase11ProgressionVisibility.Services.Animals;
public interface IAnimalRepository
{
    Task<BasicList<AnimalAutoResumeModel>> LoadAsync();
    Task SaveAsync(BasicList<AnimalAutoResumeModel> animals);
}