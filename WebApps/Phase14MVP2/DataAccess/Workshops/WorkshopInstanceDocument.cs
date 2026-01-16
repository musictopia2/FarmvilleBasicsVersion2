using Phase14MVP2.Services.Core;

namespace Phase14MVP2.DataAccess.Workshops;
public class WorkshopInstanceDocument : IFarmDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<WorkshopAutoResumeModel> Workshops { get; set; }
}