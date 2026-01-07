namespace Phase01IntroduceBarnAndSilo.DataAccess.Core;
public class StartingFactory : IStartingFactory
{
    IInventoryRepository IStartingFactory.GetInventoryServices(FarmKey farm)
    {
        return new InventoryDatabase();
    }
}