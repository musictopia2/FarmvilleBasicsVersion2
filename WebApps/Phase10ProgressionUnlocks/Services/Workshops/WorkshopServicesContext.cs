namespace Phase10ProgressionUnlocks.Services.Workshops;
public class WorkshopServicesContext
{
    required public IWorkshopRegistry WorkshopRegistry { get; init; }
    required public IWorkshopInstances WorkshopInstances { get; init; }
    required public IWorkshopProgressionPolicy WorkshopProgressionPolicy { get; init; }
    required public IWorkshopCollectionPolicy WorkshopCollectionPolicy { get; init; }
    required public IWorkshopPersistence WorkshopPersistence { get; init; }
}