namespace Phase10ProgressionUnlocks.DataAccess.Workshops;
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
            WorkshopRegistry = register,
            WorkshopRespository = instance
        };
        return output;
    }
}