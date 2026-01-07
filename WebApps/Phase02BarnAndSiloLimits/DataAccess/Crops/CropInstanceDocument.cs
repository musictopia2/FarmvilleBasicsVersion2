using Phase02BarnAndSiloLimits.Services.Core;

namespace Phase02BarnAndSiloLimits.DataAccess.Crops;
public class CropInstanceDocument
{
    required public BasicList<CropAutoResumeModel> Slots { get; set; } = [];
    required public FarmKey Farm { get; set; }
}