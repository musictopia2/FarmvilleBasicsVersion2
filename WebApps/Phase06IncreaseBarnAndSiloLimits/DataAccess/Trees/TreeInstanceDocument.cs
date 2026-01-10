using Phase06IncreaseBarnAndSiloLimits.Services.Core;

namespace Phase06IncreaseBarnAndSiloLimits.DataAccess.Trees;
public class TreeInstanceDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<TreeAutoResumeModel> Trees { get; set; }
}