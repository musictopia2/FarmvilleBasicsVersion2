using Phase06IncreaseBarnAndSiloLimits.Services.Core;

namespace Phase06IncreaseBarnAndSiloLimits.DataAccess.Animals;
public class AnimalInstanceDocument
{
    required public BasicList<AnimalAutoResumeModel> Animals { get; set; }
    required public FarmKey Farm { get; set; }
}