using Phase04EnforcingLimits.Services.Core;

namespace Phase04EnforcingLimits.DataAccess.Crops;
public class CropInstanceDocument
{
    required public BasicList<CropAutoResumeModel> Slots { get; set; } = [];
    required public FarmKey Farm { get; set; }
}