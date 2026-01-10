using Phase07EarnCoinFromQuests.Services.Core;

namespace Phase07EarnCoinFromQuests.DataAccess.Worksites;
public class WorksiteInstanceDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<WorksiteAutoResumeModel> Worksites { get; set; } = [];
}