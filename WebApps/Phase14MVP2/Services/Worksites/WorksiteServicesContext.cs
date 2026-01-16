namespace Phase14MVP2.Services.Worksites;
public class WorksiteServicesContext
{
    required public IWorksiteRegistry WorksiteRegistry { get; init; }
    required public IWorksiteRepository WorksiteRepository { get; init; }
    required public IWorksiteCollectionPolicy WorksiteCollectionPolicy { get; init; }
}