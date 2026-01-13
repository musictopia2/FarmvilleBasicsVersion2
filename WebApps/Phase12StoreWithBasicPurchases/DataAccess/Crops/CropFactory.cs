namespace Phase12StoreWithBasicPurchases.DataAccess.Crops;
public class CropFactory : ICropFactory
{
    CropServicesContext ICropFactory.GetCropServices(FarmKey farm)
    {
        ICropHarvestPolicy collection;
        collection = new CropManualHarvestPolicy();
        ICropRegistry register;
        register = new CropRecipeDatabase(farm);
        CropInstanceDatabase db = new(farm);
        CropServicesContext output = new()
        {
            CropHarvestPolicy = collection,
            CropRegistry = register,
            CropRepository = db,
        };
        return output;
    }
}