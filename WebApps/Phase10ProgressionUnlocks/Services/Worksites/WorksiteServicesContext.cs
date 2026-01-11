namespace Phase10ProgressionUnlocks.Services.Worksites;
public class WorksiteServicesContext
{
    required public IWorksiteRegistry WorksiteRegistry { get; init; }
    required public IWorksiteInstances WorksiteInstances { get; init; }
    required public IWorksiteProgressPolicy WorksiteProgressPolicy { get; init; }
    required public IWorksiteCollectionPolicy WorksiteCollectionPolicy { get; init; }
    required public IWorksitePersistence WorksitePersistence { get; init; }
}