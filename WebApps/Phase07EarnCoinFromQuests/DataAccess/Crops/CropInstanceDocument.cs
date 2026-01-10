using Phase07EarnCoinFromQuests.Services.Core;

namespace Phase07EarnCoinFromQuests.DataAccess.Crops;
public class CropInstanceDocument
{
    required public BasicList<CropAutoResumeModel> Slots { get; set; } = [];
    required public FarmKey Farm { get; set; }
}