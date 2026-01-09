using Phase04EnforcingLimits.Services.Core;

namespace Phase04EnforcingLimits.DataAccess.Workshops;
public class WorkshopInstanceDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<WorkshopAutoResumeModel> Workshops { get; set; }
}