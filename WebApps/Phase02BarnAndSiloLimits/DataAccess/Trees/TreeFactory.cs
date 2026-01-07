using Phase02BarnAndSiloLimits.Services.Core;

namespace Phase02BarnAndSiloLimits.DataAccess.Trees;
public class TreeFactory : ITreeFactory
{
    TreeServicesContext ITreeFactory.GetTreeServices(FarmKey farm)
    {
        ITreeGatheringPolicy collection;
        collection = new TreeGatherAllPolicy();
        ITreeRecipes register;
        register = new TreeRecipeDatabase(farm);
        TreeInstanceDatabase instance = new(farm);
        TreeServicesContext output = new()
        {
            TreeGatheringPolicy = collection,
            TreeProgressionPolicy = new BasicTreePolicy(),
            TreeRegistry = register,
            TreeInstances = instance,
            TreesCollecting = new DefaultTreesCollected(),
            TreePersistence = instance
        };
        return output;
    }   
}