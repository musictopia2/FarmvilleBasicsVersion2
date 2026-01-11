using Phase10ProgressionUnlocks.Services.Core;

namespace Phase10ProgressionUnlocks.DataAccess.Animals;
public class AnimalInstanceDocument
{
    required public BasicList<AnimalAutoResumeModel> Animals { get; set; }
    required public FarmKey Farm { get; set; }
}