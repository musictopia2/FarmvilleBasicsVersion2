using Phase07EarnCoinFromQuests.Services.Core;

namespace Phase07EarnCoinFromQuests.DataAccess.Trees;
public class TreeInstanceDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<TreeAutoResumeModel> Trees { get; set; }
}