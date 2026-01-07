namespace Phase01IntroduceBarnAndSilo.Services.Core;
public interface IStartingFactory
{
    IInventoryRepository GetInventoryServices(FarmKey farm);
}