using Phase11ProgressionVisibility.Services.Core;

namespace Phase11ProgressionVisibility.DataAccess.Workshops;
public class WorkshopInstanceDocument : IFarmDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<WorkshopAutoResumeModel> Workshops { get; set; }
}