using Phase13QuestsBasedOnLevel.Services.Core;

namespace Phase13QuestsBasedOnLevel.DataAccess.Workshops;
public class WorkshopInstanceDocument : IFarmDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<WorkshopAutoResumeModel> Workshops { get; set; }
}