namespace Phase10ProgressionUnlocks.DataAccess.Workers;
public class WorkerInstanceDocument : IFarmDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<WorkerDataModel> Workers { get; set; } = [];
}
