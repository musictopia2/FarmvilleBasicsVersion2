using Phase02BarnAndSiloLimits.Services.Core;

namespace Phase02BarnAndSiloLimits.DataAccess.Animals;
public class AnimalInstanceDocument
{
    required public BasicList<AnimalAutoResumeModel> Animals { get; set; }
    required public FarmKey Farm { get; set; }
}