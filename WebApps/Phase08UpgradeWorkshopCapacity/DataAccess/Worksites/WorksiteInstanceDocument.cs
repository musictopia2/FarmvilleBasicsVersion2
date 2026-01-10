using Phase08UpgradeWorkshopCapacity.Services.Core;

namespace Phase08UpgradeWorkshopCapacity.DataAccess.Worksites;
public class WorksiteInstanceDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<WorksiteAutoResumeModel> Worksites { get; set; } = [];
}