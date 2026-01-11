using Phase10ProgressionUnlocks.Services.Core;

namespace Phase10ProgressionUnlocks.DataAccess.Worksites;
public class WorksiteFactory : IWorksiteFactory
{
    WorksiteServicesContext IWorksiteFactory.GetWorksiteServices(FarmKey farm)
    {
        IWorksiteCollectionPolicy collection;
        collection = new WorksiteManualCollectionPolicy();
        
        WorksiteInstanceDatabase instance = new(farm);

        IWorksiteRegistry register;
        register = new WorksiteRecipeDatabase(farm);
        WorksiteServicesContext output = new()
        {
            WorksiteCollectionPolicy = collection,
            WorksiteProgressPolicy = new BasicWorksitePolicy(),
            WorksiteRegistry = register,
            WorksiteInstances = instance,
            WorksitePersistence  = instance
        };
        return output;
    }   
}