namespace Phase06IncreaseBarnAndSiloLimits.Services.Animals;
public interface IAnimalRegistry
{
    Task<BasicList<AnimalRecipe>> GetAnimalsAsync();
}