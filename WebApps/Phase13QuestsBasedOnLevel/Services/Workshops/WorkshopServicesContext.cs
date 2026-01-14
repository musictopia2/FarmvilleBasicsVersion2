namespace Phase13QuestsBasedOnLevel.Services.Workshops;
public class WorkshopServicesContext
{
    required public IWorkshopRegistry WorkshopRegistry { get; init; }
    required public IWorkshopRespository WorkshopRespository { get; init; }
    required public IWorkshopCollectionPolicy WorkshopCollectionPolicy { get; init; }
}