using Phase10ProgressionUnlocks.Services.Core;

namespace Phase10ProgressionUnlocks.DataAccess.Trees;
public class TreeInstanceDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<TreeAutoResumeModel> Trees { get; set; }
}