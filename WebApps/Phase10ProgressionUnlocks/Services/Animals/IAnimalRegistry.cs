namespace Phase10ProgressionUnlocks.Services.Animals;
public interface IAnimalRegistry
{
    Task<BasicList<AnimalRecipe>> GetAnimalsAsync();
}