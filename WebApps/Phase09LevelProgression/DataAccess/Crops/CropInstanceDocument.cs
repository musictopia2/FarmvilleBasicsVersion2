using Phase09LevelProgression.Services.Core;

namespace Phase09LevelProgression.DataAccess.Crops;
public class CropInstanceDocument
{
    required public BasicList<CropAutoResumeModel> Slots { get; set; } = [];
    required public FarmKey Farm { get; set; }
}