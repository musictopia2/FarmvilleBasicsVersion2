namespace Phase05BarnSiloLimits.Models;
public class AnimalInstanceDocument
{
    required public BasicList<AnimalAutoResumeModel> Animals { get; set; }
    required public FarmKey Farm { get; set; }
}