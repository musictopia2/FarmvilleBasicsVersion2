namespace Phase10ProgressionUnlocks.Models;
public class WorkerInstanceDocument : IFarmDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<WorkerDataModel> Workers { get; set; } = [];
}
