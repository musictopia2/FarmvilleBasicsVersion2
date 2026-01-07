namespace Phase01IntroduceBarnAndSilo.Services.Crops;
public class CropServicesContext
{
    required public ICropRegistry CropRegistry{ get; init; }
    required public ICropInstances CropInstances { get; init; }
    required public ICropProgressionPolicy CropProgressionPolicy { get; init; }
    required public ICropHarvestPolicy CropHarvestPolicy { get; init; }
    required public ICropPersistence CropPersistence { get; init; }
}