using Phase02BarnAndSiloLimits.Services.Core;

namespace Phase02BarnAndSiloLimits.DataAccess.Trees;
public class TreeInstanceDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<TreeAutoResumeModel> Trees { get; set; }
}