using Phase05IntroduceCoins.Services.Core;

namespace Phase05IntroduceCoins.DataAccess.Trees;
public class TreeInstanceDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<TreeAutoResumeModel> Trees { get; set; }
}