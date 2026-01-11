namespace Phase10ProgressionUnlocks.Services.Crops;
public class CropSystemState
{
    public BasicList<CropAutoResumeModel> Slots { get; set; } = [];
    public BasicList<CropDataModel> Crops { get; set; } = [];
}