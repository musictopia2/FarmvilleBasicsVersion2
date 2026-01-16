using Phase14MVP2.Services.Core;

namespace Phase14MVP2.DataAccess.Worksites;
public class WorksiteInstanceDocument : IFarmDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<WorksiteAutoResumeModel> Worksites { get; set; } = [];
}