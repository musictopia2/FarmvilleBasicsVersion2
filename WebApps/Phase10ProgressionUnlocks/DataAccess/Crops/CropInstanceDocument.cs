using Phase10ProgressionUnlocks.Services.Core;

namespace Phase10ProgressionUnlocks.DataAccess.Crops;
public class CropInstanceDocument
{
    required public BasicList<CropAutoResumeModel> Slots { get; set; } = [];
    required public FarmKey Farm { get; set; }
}