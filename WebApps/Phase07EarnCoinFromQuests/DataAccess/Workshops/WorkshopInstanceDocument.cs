using Phase07EarnCoinFromQuests.Services.Core;

namespace Phase07EarnCoinFromQuests.DataAccess.Workshops;
public class WorkshopInstanceDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<WorkshopAutoResumeModel> Workshops { get; set; }
}