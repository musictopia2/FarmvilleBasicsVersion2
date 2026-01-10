using Phase08UpgradeWorkshopCapacity.Services.Core;

namespace Phase08UpgradeWorkshopCapacity.DataAccess.Animals;
public class AnimalInstanceDocument
{
    required public BasicList<AnimalAutoResumeModel> Animals { get; set; }
    required public FarmKey Farm { get; set; }
}