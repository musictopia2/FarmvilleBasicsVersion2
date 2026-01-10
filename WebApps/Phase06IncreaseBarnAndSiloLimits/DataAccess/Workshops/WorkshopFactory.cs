using Phase06IncreaseBarnAndSiloLimits.Services.Core;

namespace Phase06IncreaseBarnAndSiloLimits.DataAccess.Workshops;
public class WorkshopFactory : IWorkshopFactory
{
    WorkshopServicesContext IWorkshopFactory.GetWorkshopServices(FarmKey farm)
    {
        IWorkshopCollectionPolicy collection;
        collection = new WorkshopManualCollectionPolicy();
        IWorkshopRegistry register;
        register = new WorkshopRecipeDatabase(farm);
        WorkshopInstanceDatabase instance = new(farm);
        WorkshopServicesContext output = new()
        {
            WorkshopCollectionPolicy = collection,
            WorkshopProgressionPolicy = new BasicWorkshopPolicy(),
            WorkshopRegistry = register,
            WorkshopInstances = instance,
            WorkshopPersistence = instance
        };
        return output;
    }   
}