namespace Phase10ProgressionUnlocks.DataAccess.Crops;
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
            CropProgressionPolicy = new BasicCropPolicy(),
            CropRegistry = register,
            CropInstances = db,
            CropPersistence = db
        };
        return output;
    }
}