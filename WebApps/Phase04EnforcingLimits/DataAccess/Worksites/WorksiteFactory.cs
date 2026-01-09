using Phase04EnforcingLimits.Services.Core;

namespace Phase04EnforcingLimits.DataAccess.Worksites;
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