using Phase13QuestsBasedOnLevel.Services.Core;

namespace Phase13QuestsBasedOnLevel.DataAccess.Trees;
public class TreeInstanceDocument : IFarmDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<TreeAutoResumeModel> Trees { get; set; }
}