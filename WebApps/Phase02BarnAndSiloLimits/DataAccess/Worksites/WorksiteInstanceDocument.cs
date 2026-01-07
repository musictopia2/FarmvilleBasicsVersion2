using Phase02BarnAndSiloLimits.Services.Core;

namespace Phase02BarnAndSiloLimits.DataAccess.Worksites;
public class WorksiteInstanceDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<WorksiteAutoResumeModel> Worksites { get; set; } = [];
}