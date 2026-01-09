using Phase04EnforcingLimits.Services.Core;

namespace Phase04EnforcingLimits.DataAccess.Trees;
public class TreeInstanceDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<TreeAutoResumeModel> Trees { get; set; }
}