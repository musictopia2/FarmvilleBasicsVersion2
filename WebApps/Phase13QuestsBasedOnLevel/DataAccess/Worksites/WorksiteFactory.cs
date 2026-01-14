using Phase13QuestsBasedOnLevel.Services.Core;

namespace Phase13QuestsBasedOnLevel.DataAccess.Worksites;
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
            WorksiteRegistry = register,
            WorksiteRepository = instance
        };
        return output;
    }   
}