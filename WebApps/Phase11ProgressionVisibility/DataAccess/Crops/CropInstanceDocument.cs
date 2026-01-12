namespace Phase11ProgressionVisibility.DataAccess.Crops;
public class CropInstanceDocument : IFarmDocument
{
    required public BasicList<CropAutoResumeModel> Slots { get; set; } = [];
    required public BasicList<CropDataModel> Crops { get; set; } = [];
    required public FarmKey Farm { get; set; }
}