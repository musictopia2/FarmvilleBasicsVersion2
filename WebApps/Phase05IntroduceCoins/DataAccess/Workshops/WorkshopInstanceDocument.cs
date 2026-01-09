using Phase05IntroduceCoins.Services.Core;

namespace Phase05IntroduceCoins.DataAccess.Workshops;
public class WorkshopInstanceDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<WorkshopAutoResumeModel> Workshops { get; set; }
}