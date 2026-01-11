using Phase10ProgressionUnlocks.Services.Core;

namespace Phase10ProgressionUnlocks.DataAccess.Workshops;
public class WorkshopInstanceDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<WorkshopAutoResumeModel> Workshops { get; set; }
}