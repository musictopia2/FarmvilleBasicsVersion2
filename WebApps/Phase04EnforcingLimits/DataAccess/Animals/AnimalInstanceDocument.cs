using Phase04EnforcingLimits.Services.Core;

namespace Phase04EnforcingLimits.DataAccess.Animals;
public class AnimalInstanceDocument
{
    required public BasicList<AnimalAutoResumeModel> Animals { get; set; }
    required public FarmKey Farm { get; set; }
}