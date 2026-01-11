using Phase10ProgressionUnlocks.Services.Core;

namespace Phase10ProgressionUnlocks.DataAccess.Worksites;
public class WorksiteInstanceDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<WorksiteAutoResumeModel> Worksites { get; set; } = [];
}